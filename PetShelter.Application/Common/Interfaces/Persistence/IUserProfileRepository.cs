using DomainUserProfile = PetShelter.Domain.Entities.UserProfile;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IUserProfileRepository
{
    Task<DomainUserProfile?> GetByIdAsync(Guid id);
    Task AddAsync(DomainUserProfile profile);
    Task UpdateAsync(DomainUserProfile profile);
}
