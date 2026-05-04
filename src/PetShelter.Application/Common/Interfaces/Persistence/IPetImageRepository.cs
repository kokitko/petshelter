using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IPetImageRepository
{
    Task AddAsync(PetImage petImage);
    Task DeleteAsync(PetImage petImage);
}
