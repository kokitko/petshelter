using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IAppUserRepository
{
    Task AddAsync(AppUser user);
    Task UpdateAsync(AppUser user);
    Task<AppUser?> GetByEmailAsync(string email);
    Task<AppUser?> GetByIdAsync(Guid id);
    Task DeleteAsync(AppUser user);
}
