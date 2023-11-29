using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RealWorldConduit.Infrastructure.Extensions
{
    public static class DatabaseConfigurationExtension
    {
        public static IServiceCollection DatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("MainDbContextString")));
            return services;
        }
    }
}
