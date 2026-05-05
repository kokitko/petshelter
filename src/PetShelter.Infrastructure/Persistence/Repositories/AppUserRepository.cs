using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class AppUserRepository(
    PetShelterDbContext context,
    ILogger<AppUserRepository> logger
    ) : IAppUserRepository
{
    public async Task AddAsync(AppUser user)
    {
        try {
            context.AppUsers.Add(user);
            await context.SaveChangesAsync();
            logger.LogInformation("User added successfully with id: {UserId}", user.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding user with email: {Email}", user.Email);
            throw;
        }
    }

    public async Task<AppUser?> GetByEmailAsync(string email)
    {
        try {
        var user = await context.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        logger.LogInformation("Retrieved user with email: {Email}", user?.Email);
        return user;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving user with email: {Email}", email);
            throw;
        }
    }

    public async Task<AppUser?> GetByIdAsync(Guid id)
    {
        try {
            var user = await context.AppUsers
                .Include(u => u.OrgProfile)
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.Id == id);

            logger.LogInformation("Retrieved user with id: {UserId}", user?.Id);
            return user;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving user with id: {UserId}", id);
            throw;
        }
    }

    public async Task UpdateAsync(AppUser user)
    {
        try {
            context.AppUsers.Update(user);
            await context.SaveChangesAsync();
            logger.LogInformation("User updated successfully with id: {UserId}", user.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error updating user with id: {UserId}", user.Id);
            throw;
        }
    }

    public async Task DeleteAsync(AppUser user)
    {
        try {
            context.AppUsers.Remove(user);
            await context.SaveChangesAsync();
            logger.LogInformation("User deleted successfully with id: {UserId}", user.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting user with id: {UserId}", user.Id);
            throw;
        }
    }
}
