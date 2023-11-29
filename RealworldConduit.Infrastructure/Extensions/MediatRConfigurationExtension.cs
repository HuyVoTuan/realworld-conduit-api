using Microsoft.Extensions.DependencyInjection;

namespace RealworldConduit.Infrastructure.Extensions
{
    public static class MediatRConfigurationExtension
    {
        public static IServiceCollection MediatRConfiguration(this IServiceCollection services)
        {
            var executingAssembly = AppDomain.CurrentDomain.Load("RealWorldConduit.Application");
            services.AddMediatR(cfg =>
                     cfg.RegisterServicesFromAssemblies(executingAssembly));

            return services;
        }
    }
}
