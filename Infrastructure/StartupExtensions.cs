using Core.Contracts;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class StartupExtensions
    {
        public static void AddRepositoryServices(this IServiceCollection services)
        {
            services.AddTransient<INoticiaRepository, NoticiaRepository>();
            services.AddTransient<IComentarioRepository, ComentarioRepository>();
        }
    }
}
