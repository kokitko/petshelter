using Microsoft.Extensions.DependencyInjection;
using MediatR;
using PetShelter.Application.Common.Behaviours;
using FluentValidation;
using System.Reflection;
using PetShelter.Application.Common.Logging;

namespace PetShelter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehaviour<,>));

        services.AddScoped(
            typeof(IPipelineBehavior<,>), 
            typeof(ValidationBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}