using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using PetShelter.Infrastructure.Persistence;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Infrastructure.Persistence.Repositories;

namespace PetShelter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .AddPersistence();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services)
    {
        services
            .AddDbContext<PetShelterDbContext>(options => 
                options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=PetShelterDb;Trusted_Connection=true;TrustServerCertificate=true"));

        services.AddScoped<IPetRepository, PetRepository>();
        
        return services;
    }
}
