using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace RealworldConduit.Infrastructure.Extensions
{
    public static class FluentValidationConfigurationExtension
    {
        public static IServiceCollection FluentValidationConfiguration(this IServiceCollection services)
        {
            var executingAssembly = AppDomain.CurrentDomain.Load("RealWorldConduit.Application");
            services.AddValidatorsFromAssembly(executingAssembly).AddFluentValidationAutoValidation();
            return services;
        }
    }
}
