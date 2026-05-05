using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IPetRepository
{
    Task<(IEnumerable<Pet> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize,
        PetStatus? status,
        string? species, 
        string? breed,
        string? name,
        int? age,
        Guid? ownerId);
    Task<Pet?> GetByIdAsync(Guid id);
    Task AddAsync(Pet pet);
    Task SaveChangesAsync();
    Task DeleteAsync(Pet pet);
}
