using Microsoft.EntityFrameworkCore;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class AdoptionApplicationRepository(
    PetShelterDbContext context
) : IAdoptionApplicationRepository
{
    public async Task AddAsync(AdoptionApplication application)
    {
        context.AdoptionApplications.Add(application);
        await context.SaveChangesAsync();
    }

    public async Task<AdoptionApplication?> GetByIdAsync(Guid id)
    {
        var application = await context.AdoptionApplications
            .Include(a => a.Pet)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.Id == id);

        return application;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
