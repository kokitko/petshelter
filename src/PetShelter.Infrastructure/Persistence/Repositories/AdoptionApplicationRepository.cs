using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class AdoptionApplicationRepository(
    PetShelterDbContext context,
    ILogger<AdoptionApplicationRepository> logger
) : IAdoptionApplicationRepository
{
    public async Task AddAsync(AdoptionApplication application)
    {
        try {
            context.AdoptionApplications.Add(application);
            await context.SaveChangesAsync();
            logger.LogInformation("Added new adoption application with id: {ApplicationId} for petId: {PetId} by applicantId: {ApplicantId}", application.Id, application.PetId, application.ApplicantId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding adoption application for petId: {PetId} by applicantId: {ApplicantId}", application.PetId, application.ApplicantId);
            throw;
        }
    }

    public async Task<AdoptionApplication?> GetByIdAsync(Guid id)
    {
        try {
            var application = await context.AdoptionApplications
                .Include(a => a.Pet)
                .Include(a => a.Applicant)
                .FirstOrDefaultAsync(a => a.Id == id);

            logger.LogInformation("Retrieved adoption application by id: {ApplicationId} for petId: {PetId} by applicantId: {ApplicantId}", id, application?.PetId, application?.ApplicantId);
            return application;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving adoption application by id: {ApplicationId}", id);
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        try {
            await context.SaveChangesAsync();
            logger.LogInformation("Saved changes to adoption applications");
        } catch (Exception ex) {
            logger.LogError(ex, "Error saving changes to adoption applications");
            throw;
        }
    }

    public async Task DeleteAsync(AdoptionApplication application)
    {
        try {
            context.AdoptionApplications.Remove(application);
            await context.SaveChangesAsync();
            logger.LogInformation("Deleted adoption application with id: {ApplicationId}", application.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting adoption application with id: {ApplicationId}", application.Id);
            throw;
        }
    }

    public async Task<AdoptionApplication?> GetApprovedByPetIdAsync(Guid petId)
    {
        try {
            var application = await context.AdoptionApplications
                .Include(a => a.Pet)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.PetId == petId && a.Status == ApplicationStatus.Approved);

            logger.LogInformation("Retrieved approved adoption application for petId: {PetId} with applicationId: {ApplicationId} by applicantId: {ApplicantId}", petId, application?.Id, application?.ApplicantId);
            return application;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving approved adoption application for petId: {PetId}", petId);
            throw;
        }
    }

    public async Task<IEnumerable<AdoptionApplication>> GetByPetIdAsync(Guid petId)
    {
        try {
            var applications = await context.AdoptionApplications
                .Include(a => a.Pet)
                .Include(a => a.Applicant)
                .Where(a => a.PetId == petId)
                .ToListAsync();

            logger.LogInformation("Retrieved {Count} adoption applications for petId: {PetId}", applications.Count, petId);
            return applications;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving adoption applications for petId: {PetId}", petId);
            throw;
        }
    }

    public async Task<(List<AdoptionApplication>, int)> GetByApplicantIdAsync(Guid applicantId, ApplicationStatus? status, int pageNumber, int pageSize)
    {
        try {
            var query = context.AdoptionApplications
                .Include(a => a.Pet)
                .Include(a => a.Applicant)
                .Where(a => a.ApplicantId == applicantId);

            if (status.HasValue)
                query = query.Where(a => a.Status == status);

            var applications = await query
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            logger.LogInformation("Retrieved {Count} adoption applications for applicantId: {ApplicantId} with status: {Status}", applications.Count, applicantId, status);
            return (applications, totalCount);
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving adoption applications for applicantId: {ApplicantId} with status: {Status}", applicantId, status);
            throw;
        }
    }

    public async Task<(List<AdoptionApplication>, int)> GetByOwnerIdAsync(Guid ownerId, ApplicationStatus? status, int pageNumber, int pageSize)
    {
        try {
            var query = context.AdoptionApplications
                .Where(a => a.Pet.OwnerId == ownerId);

            if (status.HasValue)
                query = query.Where(a => a.Status == status);

            var applications = await query
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await query.CountAsync();

            logger.LogInformation("Retrieved {Count} adoption applications for ownerId: {OwnerId} with status: {Status}", applications.Count, ownerId, status);
            return (applications, totalCount);
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving adoption applications for ownerId: {OwnerId} with status: {Status}", ownerId, status);
            throw;
        }
    }

    public async Task DeleteByPetIdAsync(Guid petId)
    {
        try {
            var applications = await GetByPetIdAsync(petId);
            context.AdoptionApplications.RemoveRange(applications);
            await context.SaveChangesAsync();
            logger.LogInformation("Deleted {Count} adoption applications for petId: {PetId}", applications.Count(), petId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting adoption applications for petId: {PetId}", petId);
            throw;
        }
    }
}
