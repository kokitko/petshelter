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

    public async Task<(IEnumerable<Pet> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? species)
    {
        var query = context.Pets.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(species))
            query = query.Where(p => p.Species == species);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }
}
