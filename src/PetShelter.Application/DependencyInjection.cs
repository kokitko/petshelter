using Microsoft.Extensions.DependencyInjection;
using MediatR;
using PetShelter.Application.Common.Behaviours;
using FluentValidation;
using System.Reflection;

namespace PetShelter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped(
            typeof(IPipelineBehavior<,>), 
            typeof(ValidationBehaviour<,>));


        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}