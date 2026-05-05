using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class PetImageRepository(
    PetShelterDbContext context,
    ILogger<PetImageRepository> logger
) : IPetImageRepository
{
    public async Task AddAsync(PetImage petImage)
    {
        try {
            context.PetImages.Add(petImage);
            await context.SaveChangesAsync();
            logger.LogInformation("Pet image added successfully with id: {ImageId} for petId: {PetId}", petImage.Id, petImage.PetId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding pet image for petId: {PetId}", petImage.PetId);
            throw;
        }
    }

    public async Task DeleteAsync(PetImage petImage)
    {
        try {
            context.PetImages.Remove(petImage);
            await context.SaveChangesAsync();
            logger.LogInformation("Pet image deleted successfully with id: {ImageId} for petId: {PetId}", petImage.Id, petImage.PetId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting pet image with id: {ImageId} for petId: {PetId}", petImage.Id, petImage.PetId);
            throw;
        }
    }
}
