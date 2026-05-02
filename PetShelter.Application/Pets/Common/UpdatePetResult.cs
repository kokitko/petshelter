namespace PetShelter.Application.Pets.Common;

public record UpdatePetResult(
    Guid Id,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<PetImageResult> PictureUrls
);
