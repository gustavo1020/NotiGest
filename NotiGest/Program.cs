using AspNetCoreRateLimit;
using NotiGest;
using NotiGest.Middleware;
using Infrastructure;
using Application;
using StartupExtensions = NotiGest.StartupExtensions;
using Hangfire;
using NotiGest.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUsersConfiguration();
builder.Services.AddDBConfiguration(builder);
builder.Services.AddCorsConfiguration(builder);
builder.Services.AddServicesRateLimiting(builder);
builder.Services.AddServicesAuth(builder);
builder.Services.AddRepositoryServices();
builder.Services.AddServices();
builder.Services.AddFluentValidationd();
builder.Services.AddMapster();
builder.Services.AddHangfireServer();
builder.Services.AddRedis(builder);
builder.Services.AddHangfireService();
builder.Services.AddRestService();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CrossOriginResourceSharingPolicy");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "NotiGest",
    DefaultRecordsPerPage = 10,
    Authorization = new[] { new HangfireAuthorization(true) }
});

StartupExtensions.AddUsersConfig(app);
app.UseIpRateLimiting();

app.MapControllers();

app.Run();
