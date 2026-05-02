namespace PetShelter.Application.Pets.Common;

public record PetResult(
    Guid Id,
    Guid OwnerId,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<PetImageResult> PicturesInfo
);
