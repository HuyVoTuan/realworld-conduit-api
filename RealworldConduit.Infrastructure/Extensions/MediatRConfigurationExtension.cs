using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealworldConduit.Infrastructure.Behaviors;
using RealworldConduit.Infrastructure.Filters;

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

        public static IServiceCollection MediatRPipelinesConfiguration(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
            return services;
        }
    }
}
