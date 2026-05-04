using Microsoft.EntityFrameworkCore;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class PetRepository(PetShelterDbContext context) : IPetRepository
{
    public async Task AddAsync(Pet pet)
    {
        context.Pets.Add(pet);
        await context.SaveChangesAsync();
    }

    public async Task<Pet?> GetByIdAsync(Guid id)
    {
        var pet = await context.Pets
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

        return pet;
    }

    public async Task<(IEnumerable<Pet> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? species, 
        string? breed,
        string? name,
        int? age,
        Guid? ownerId)
    {
        var query = context.Pets.AsNoTracking().AsQueryable();

        if (ownerId.HasValue)
            query = query.Where(p => p.OwnerId == ownerId.Value);
        if (!string.IsNullOrEmpty(species))
            query = query.Where(p => p.Species.Contains(species));
        if (!string.IsNullOrEmpty(breed))
            query = query.Where(p => p.Breed.Contains(breed));
        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));
        if (age.HasValue)
            query = query.Where(p => p.Age == age.Value);

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pet pet)
    {
        context.Pets.Remove(pet);
        await context.SaveChangesAsync();
    }
}
