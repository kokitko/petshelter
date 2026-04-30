using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class UserProfileRepository(PetShelterDbContext context) : IUserProfileRepository
{
    public async Task AddAsync(UserProfile profile)
    {
        context.UserProfiles.Add(profile);
        await context.SaveChangesAsync();
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id)
    {
        var profile = await context.UserProfiles
            .FindAsync(id);

        return profile;
    }

    public async Task UpdateAsync(UserProfile profile)
    {
        context.UserProfiles.Update(profile);
        await context.SaveChangesAsync();
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
    {
        var profile = await context.UserProfiles.FindAsync(userId);

        return profile;
    }
}
