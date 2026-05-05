using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class PetRepository(
    PetShelterDbContext context,
    ILogger<PetRepository> logger
) : IPetRepository
{
    public async Task AddAsync(Pet pet)
    {
        try {
            context.Pets.Add(pet);
            await context.SaveChangesAsync();
            logger.LogInformation("Pet added successfully with id: {PetId}", pet.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding pet for userId: {UserId}", pet.OwnerId);
            throw;
        }
    }

    public async Task<Pet?> GetByIdAsync(Guid id)
    {
        try {
            var pet = await context.Pets
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            logger.LogInformation("Retrieved pet with id: {PetId}", pet?.Id);
            return pet;
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving pet with id: {PetId}", id);
            throw;
        }
    }

    public async Task<(IEnumerable<Pet> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        PetStatus? status,
        string? species, 
        string? breed,
        string? name,
        int? age,
        Guid? ownerId)
    {
        try {
            var query = context.Pets.AsNoTracking().Include(p => p.Images).AsQueryable();
            
            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);
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
            
            logger.LogInformation("Retrieved paged pets with filters - status: {Status}, species: {Species}, breed: {Breed}, name: {Name}, age: {Age}, ownerId: {OwnerId}, pageNumber: {PageNumber}, pageSize: {PageSize}. TotalCount: {TotalCount}", 
                status, species, breed, name, age, ownerId, pageNumber, pageSize, totalCount);
            return (items, totalCount);
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving paged pets with filters - status: {Status}, species: {Species}, breed: {Breed}, name: {Name}, age: {Age}, ownerId: {OwnerId}, pageNumber: {PageNumber}, pageSize: {PageSize}", 
                status, species, breed, name, age, ownerId, pageNumber, pageSize);
            throw;
        }
    }

    public async Task SaveChangesAsync()
    {
        try {
            await context.SaveChangesAsync();
            logger.LogInformation("Changes to pets saved successfully");
        } catch (Exception ex) {
            logger.LogError(ex, "Error saving changes to pets");
            throw;
        }
    }

    public async Task DeleteAsync(Pet pet)
    {
        try {
            context.Pets.Remove(pet);
            await context.SaveChangesAsync();
            logger.LogInformation("Pet deleted successfully with id: {PetId}", pet.Id);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting pet with id: {PetId}", pet.Id);
            throw;
        }
    }
}
