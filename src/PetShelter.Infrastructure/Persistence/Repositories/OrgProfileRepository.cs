using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<(IEnumerable<OrgProfile> Profiles, int TotalCount)> GetOrgProfilesAsync(
        int pageNumber, 
        int pageSize, 
        string? OrgName, 
        string? Address)
    {
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

        return (profiles, totalCount);
    }
}
