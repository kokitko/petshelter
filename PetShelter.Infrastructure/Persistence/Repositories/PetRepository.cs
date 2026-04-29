using Microsoft.EntityFrameworkCore;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class PetRepository(PetShelterDbContext context) : IPetRepository
{
    public Task AddAsync(Pet pet)
    {
        throw new NotImplementedException();
    }

    public Task<Pet?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
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
