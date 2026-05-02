using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class OrgProfileRepository(PetShelterDbContext context) : IOrgProfileRepository
{
    public async Task AddAsync(OrgProfile profile)
    {
        context.OrgProfiles.Add(profile);
        await context.SaveChangesAsync();
    }

    public async Task<OrgProfile?> GetByIdAsync(Guid id)
    {
        var profile = await context.OrgProfiles
            .FindAsync(id);

        return profile;
    }

    public async Task UpdateAsync(OrgProfile profile)
    {
        context.OrgProfiles.Update(profile);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(OrgProfile profile)
    {
        context.OrgProfiles.Remove(profile);
        await context.SaveChangesAsync();
    }
}
