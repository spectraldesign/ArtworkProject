using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ArtworkProjectDbContext>(o => o.UseNpgsql(config.GetConnectionString("ConnectionString")));
            services.AddScoped<IArtworkProjectDbContext>(provider => provider.GetService<ArtworkProjectDbContext>());
            return services;
        }
    }
}
