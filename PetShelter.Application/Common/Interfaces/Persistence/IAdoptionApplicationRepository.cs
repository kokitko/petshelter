using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IAdoptionApplicationRepository
{
    Task AddAsync(AdoptionApplication application);
    Task<AdoptionApplication?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
