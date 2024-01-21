using Application.BackgroundService;
using Application.Contracts;
using Application.Services;
using Application.Validator;
using Core.Entityes;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotiGest.Services;
using RestSharp;
using StackExchange.Redis;
using System.Reflection;

namespace Application
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<INoticiaService, NoticiaService>();
            services.AddScoped<IComentarioService, ComentarioService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped(typeof(IRedisService<>), typeof(RedisService<>));
        }

        public static void AddFluentValidationd(this IServiceCollection services)
        {
            services.AddTransient<IValidator<Comentario>, ComentarioValidator>();
            services.AddTransient<IValidator<Noticia>, NoticiaValidator>();
        }

        public static void AddMapster(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
            var mapperConfig = new Mapper(typeAdapterConfig);
            services.AddSingleton<IMapper>(mapperConfig);
        }
        public static void AddRedis(this IServiceCollection services, WebApplicationBuilder webApplicationBuilder)
        {
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(webApplicationBuilder.Configuration.GetConnectionString("Redis") ?? "localhost:6379,abortConnect=false"));
        }

        public static void AddHangfireService(this IServiceCollection services)
        {
            services.AddScoped(typeof(ITaskNoticiaService), typeof(TaskNoticiaService));
        }

        public static void AddRestService(this IServiceCollection services)
        {
            services.AddSingleton(new RestClient(new HttpClient()));
        }
    }
}
