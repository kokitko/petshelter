using static PetShelter.Domain.Common.Errors.Errors;
using DomainUserProfile = PetShelter.Domain.Entities.UserProfile;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IUserProfileRepository
{
    Task<(IEnumerable<DomainUserProfile> Profiles, int TotalCount)> GetUserProfilesAsync(int pageNumber, int pageSize, string? FirstName, string? LastName);
    Task<DomainUserProfile?> GetByIdAsync(Guid id);
    Task AddAsync(DomainUserProfile profile);
    Task UpdateAsync(DomainUserProfile profile);
    Task DeleteAsync(DomainUserProfile profile);
}
