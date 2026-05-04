using PetShelter.Domain.Entities;

namespace PetShelter.Application.Dtos.Pet;

public record PetDto(
    Guid Id,
    Guid OwnerId,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    PetStatus Status,
    List<PetImageResult> Images
);