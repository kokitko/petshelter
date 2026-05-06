using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class UserProfileRepository(
    PetShelterDbContext context,
    ILogger<UserProfileRepository> logger
    ) : IUserProfileRepository
{
    public async Task AddAsync(UserProfile profile)
    {
        try {
            context.UserProfiles.Add(profile);
            await context.SaveChangesAsync();
            logger.LogInformation("User profile added successfully with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding user profile for userId: {UserId}", profile.UserId);
            throw;
        }
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        try {
            var profile = await context.UserProfiles
                .FindAsync(id);
            if (profile != null)            
            {
                logger.LogInformation("Retrieved user profile with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
                return profile;
            } else {
                logger.LogWarning("No user profile found with id: {ProfileId}", id);
                return null;
            }
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving user profile with id: {ProfileId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(UserProfile profile)
    {
        try {
            context.UserProfiles.Update(profile);
            await context.SaveChangesAsync();
            logger.LogInformation("User profile updated successfully with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error updating user profile with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
            throw;
        }
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
    {
        try {
            var profile = await context.UserProfiles
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile != null)            {
                logger.LogInformation("Retrieved user profile with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
                return profile;
            } else {
                logger.LogWarning("No user profile found for userId: {UserId}", userId);
                return null;
            }
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving user profile for userId: {UserId}", userId);
            throw;
        }
    }

    public async Task DeleteAsync(UserProfile profile)
    {
        try {
            context.UserProfiles.Remove(profile);
            await context.SaveChangesAsync();
            logger.LogInformation("User profile deleted successfully with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting user profile with id: {ProfileId} for userId: {UserId}", profile.Id, profile.UserId);
            throw;
        }
    }

    public async Task<(IEnumerable<UserProfile> Profiles, int TotalCount)> GetUserProfilesAsync(int pageNumber, int pageSize, string? FirstName, string? LastName)
    {
        try {
            var query = context.UserProfiles.Include(p => p.User).AsQueryable();

            if (!string.IsNullOrEmpty(FirstName))
                query = query.Where(p => p.FirstName.Contains(FirstName));

            if (!string.IsNullOrEmpty(LastName))
                query = query.Where(p => p.LastName.Contains(LastName));

            var totalCount = await query.CountAsync();
            var profiles = await query
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            logger.LogInformation("Retrieved {Count} user profiles with pageNumber: {PageNumber}, pageSize: {PageSize}, FirstName: {FirstName}, LastName: {LastName}", 
                profiles.Count, pageNumber, pageSize, FirstName, LastName);
            return (profiles, totalCount);
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving user profiles with pageNumber: {PageNumber}, pageSize: {PageSize}, FirstName: {FirstName}, LastName: {LastName}", 
                pageNumber, pageSize, FirstName, LastName);
            throw;
        }
    }
}