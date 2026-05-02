namespace PetShelter.Application.Dtos.Pet;

public record PetDto(
    Guid Id,
    Guid OwnerId,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<PetImageResult> Images
);