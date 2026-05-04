using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class PetImageRepository(
    PetShelterDbContext context
) : IPetImageRepository
{
    public async Task AddAsync(PetImage petImage)
    {
        context.PetImages.Add(petImage);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(PetImage petImage)
    {
        context.PetImages.Remove(petImage);
        await context.SaveChangesAsync();
    }
}
