using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IOrgProfileRepository
{
    Task<OrgProfile?> GetByIdAsync(Guid id);
    Task AddAsync(OrgProfile profile);
    Task UpdateAsync(OrgProfile profile);
    Task DeleteAsync(OrgProfile profile);
}
