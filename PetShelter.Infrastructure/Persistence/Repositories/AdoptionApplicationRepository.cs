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

    public async Task DeleteAsync(AdoptionApplication application)
    {
        context.AdoptionApplications.Remove(application);
        await context.SaveChangesAsync();
    }

    public Task<AdoptionApplication?> GetApprovedByPetIdAsync(Guid petId)
    {
        var application = context.AdoptionApplications
            .Include(a => a.Pet)
            .Include(a => a.Applicant)
            .FirstOrDefault(a => a.PetId == petId && a.Status == ApplicationStatus.Approved);

        return Task.FromResult(application);
    }

    public List<AdoptionApplication> GetByPetId(Guid petId)
    {
        var applications = context.AdoptionApplications
            .Where(a => a.PetId == petId)
            .ToList();

        return applications;
    }
}
