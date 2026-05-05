using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class OrgProfileRepository(
    PetShelterDbContext context,
    ILogger<OrgProfileRepository> logger
    ) : IOrgProfileRepository
{
    public async Task AddAsync(OrgProfile profile)
    {
        try {
            context.OrgProfiles.Add(profile);
            await context.SaveChangesAsync();
            logger.LogInformation("Organization profile added successfully with id: {ProfileId}", profile.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding organization profile for userId: {UserId}", profile.UserId);
            throw;
        }
    }

    public async Task<OrgProfile?> GetByIdAsync(Guid id)
    {
        try {
            var profile = await context.OrgProfiles
                .FindAsync(id);
            logger.LogInformation("Retrieved organization profile with id: {ProfileId}", profile?.Id);
            return profile;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving organization profile with id: {ProfileId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(OrgProfile profile)
    {
        try {
            context.OrgProfiles.Update(profile);
            await context.SaveChangesAsync();
            logger.LogInformation("Organization profile updated successfully with id: {ProfileId}", profile.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error updating organization profile with id: {ProfileId}", profile.Id);
            throw;
        }
    }

    public async Task DeleteAsync(OrgProfile profile)
    {
        try {
            context.OrgProfiles.Remove(profile);
            await context.SaveChangesAsync();
            logger.LogInformation("Organization profile deleted successfully with id: {ProfileId}", profile.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting organization profile with id: {ProfileId}", profile.Id);
            throw;
        }
    }

    public async Task<(IEnumerable<OrgProfile> Profiles, int TotalCount)> GetOrgProfilesAsync(
        int pageNumber, 
        int pageSize, 
        string? OrgName, 
        string? Address)
    {
        try {
            var query = context.OrgProfiles.Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(OrgName))
                query = query.Where(p => p.OrgName.Contains(OrgName));

            if (!string.IsNullOrEmpty(Address))
                query = query.Where(p => p.Address.Contains(Address));

            var totalCount = await query.CountAsync();
            var profiles = await query
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            logger.LogInformation("Retrieved {Count} organization profiles with pageNumber: {PageNumber}, pageSize: {PageSize}, OrgName: {OrgName}, Address: {Address}", 
                profiles.Count(), pageNumber, pageSize, OrgName, Address);
            return (profiles, totalCount);
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving organization profiles with pageNumber: {PageNumber}, pageSize: {PageSize}, OrgName: {OrgName}, Address: {Address}", 
                pageNumber, pageSize, OrgName, Address);
            throw;
        }
    }
}
