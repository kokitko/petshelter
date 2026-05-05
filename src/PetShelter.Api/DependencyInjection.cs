using Microsoft.AspNetCore.Mvc.Infrastructure;
using PetShelter.Api.Common.Errors;
using PetShelter.Api.Common.Exceptions;

namespace PetShelter.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, PetShelterProblemDetailsFactory>();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
        });
        return services;
    }
}
