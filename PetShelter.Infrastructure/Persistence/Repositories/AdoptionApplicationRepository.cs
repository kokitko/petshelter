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

    public async Task<AdoptionApplication?> GetApprovedByPetIdAsync(Guid petId)
    {
        var application = await context.AdoptionApplications
            .Include(a => a.Pet)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.PetId == petId && a.Status == ApplicationStatus.Approved);

        return application;
    }

    public async Task<IEnumerable<AdoptionApplication>> GetByPetIdAsync(Guid petId)
    {
        var applications = await context.AdoptionApplications
            .Where(a => a.PetId == petId)
            .ToListAsync();

        return applications;
    }

    public async Task<(List<AdoptionApplication>, int)> GetByApplicantIdAsync(Guid applicantId, ApplicationStatus? status, int pageNumber, int pageSize)
    {
        var query = context.AdoptionApplications
            .Where(a => a.ApplicantId == applicantId);

        if (status.HasValue)
            query = query.Where(a => a.Status == status);

        var applications = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await query.CountAsync();

        return (applications, totalCount);
    }

    public async Task<(List<AdoptionApplication>, int)> GetByOwnerIdAsync(Guid ownerId, ApplicationStatus? status, int pageNumber, int pageSize)
    {
        var query = context.AdoptionApplications
            .Where(a => a.Pet.OwnerId == ownerId);

        if (status.HasValue)
            query = query.Where(a => a.Status == status);

        var applications = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await query.CountAsync();

        return (applications, totalCount);
    }
}
