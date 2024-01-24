using Application.BackgroundService;
using Application.DTO;
using AspNetCoreRateLimit;
using Core.Entityes;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotiGest.Configurations;
using System.Text;

namespace NotiGest
{
    public static class StartupExtensions
    {
        public static void AddUsersConfiguration(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddRoles<Rol>().AddDefaultTokenProviders().AddEntityFrameworkStores<NotiGestDbContext>();
        }
        public static async void AddUsersConfig(this WebApplication webApplication)
        {
            RecurringJob.AddOrUpdate<TaskNoticiaService>("XXX--000", x => x.GenerarNoticias(), "0 0 31 * *");
            webApplication.MapGroup("/api/v1/Account").MapIdentityApi<User>().WithTags("Account");

            using (var scope = webApplication.Services.CreateScope())
            {
                var managerRol = scope.ServiceProvider.GetRequiredService<RoleManager<Rol>>();
                var defaultRoles = new[] { "Administrador", "UserDefault"};

                foreach (var defaultRole in defaultRoles)
                {
                    if (!await managerRol.RoleExistsAsync(defaultRole)) await managerRol.CreateAsync(new Rol(defaultRole));
                }

                var managerUser = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                if (await managerUser.FindByEmailAsync("Gustavo@admin.com") == null)
                {
                    var newUser = new User() { Email = "Gustavo@admin.com", UserName = "Gustavo" };
                    await managerUser.CreateAsync(newUser, "Password123456---");
                    await managerUser.UpdateSecurityStampAsync(newUser);
                    await managerUser.AddToRoleAsync(newUser, "Administrador");
                }
            }

        }

        public static void AddDBConfiguration(this IServiceCollection services, WebApplicationBuilder webApplication)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(webApplication.Configuration.GetConnectionString("Hangfire"), options: new SqlServerStorageOptions { PrepareSchemaIfNecessary = true }));
            var contextBuilderOptions = new DbContextOptionsBuilder<HangfireContext>();
            contextBuilderOptions.UseSqlServer(webApplication.Configuration.GetConnectionString("Hangfire"));
            var hangfireContext = new HangfireContext(contextBuilderOptions.Options);
            hangfireContext.Database.EnsureCreated();

            services.AddDbContext<NotiGestDbContext>(ops => ops.UseSqlServer(webApplication.Configuration.GetConnectionString("NotiGestConnection")));
            services.AddScoped<DbContext, NotiGestDbContext>();
            var cbo = new DbContextOptionsBuilder<NotiGestDbContext>();
            cbo.UseSqlServer(webApplication.Configuration.GetConnectionString("NotiGestConnection"));
            var context = new NotiGestDbContext(cbo.Options);
            context.Database.EnsureCreated();
        }

        public static void AddCorsConfiguration(this IServiceCollection services, WebApplicationBuilder webApplication)
        {
            var crossOriginResourceSharingPolicyConfiguration = webApplication.Configuration.GetSection("CrossOriginResourceSharingPolicy").Get<CrossOriginResourceSharingPolicyConfiguration>();

            services.AddCors(options =>
            {
                var allowedOrigins = crossOriginResourceSharingPolicyConfiguration?.AllowedOrigins?.ToArray() ?? new string[0];

                options.AddPolicy("CrossOriginResourceSharingPolicy",
                    builder => builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                );
            });
        }

        public static void AddServicesRateLimiting(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(webApplicationBuilder.Configuration.GetSection("IpRateLimiting"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void AddServicesAuth(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = webApplicationBuilder.Configuration.GetSection("Jwt:Issuer").Value,
                    ValidAudience = webApplicationBuilder.Configuration.GetSection("Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationBuilder.Configuration.GetSection("Jwt:Key").Value ?? string.Empty))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/LoginCustom";
                options.AccessDeniedPath = "/Home/Error";
                options.Cookie.Name = "token";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
            });

            services.AddAuthorization(options =>
            {
                var defaultAuth = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
                options.DefaultPolicy = defaultAuth;
            });
        }

    }
}
