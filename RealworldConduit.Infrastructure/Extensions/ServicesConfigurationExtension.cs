using Microsoft.Extensions.DependencyInjection;
using RealworldConduit.Infrastructure.Services;

namespace RealworldConduit.Infrastructure.Extensions
{
    public static class ServicesConfigurationExtension
    {
        public static IServiceCollection ServicesConfiguration(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();
            return services;
        }
    }
}
