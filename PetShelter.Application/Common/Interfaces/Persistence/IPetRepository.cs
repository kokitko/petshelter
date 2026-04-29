using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IPetRepository
{
    Task<(IEnumerable<Pet> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? species);
    Task<Pet?> GetByIdAsync(Guid id);
    Task AddAsync(Pet pet);
}
