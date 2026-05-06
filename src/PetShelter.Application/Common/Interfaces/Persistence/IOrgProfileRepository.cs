using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IOrgProfileRepository
{
    Task<(IEnumerable<OrgProfile> Profiles, int TotalCount)> GetOrgProfilesAsync(int pageNumber, int pageSize, string? OrgName, string? Address, bool? IsVerified);
    Task<OrgProfile?> GetByIdAsync(Guid id);
    Task AddAsync(OrgProfile profile);
    Task UpdateAsync(OrgProfile profile);
    Task DeleteAsync(OrgProfile profile);
}
