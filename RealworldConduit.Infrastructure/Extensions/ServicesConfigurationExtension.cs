using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealworldConduit.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
