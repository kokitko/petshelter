using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IPetRepository
{
    Task<Pet?> GetByIdAsync(Guid id);
    Task<IEnumerable<Pet>> GetAllAsync();
    Task AddAsync(Pet pet);
}
