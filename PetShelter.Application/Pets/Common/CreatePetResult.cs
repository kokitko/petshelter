namespace PetShelter.Application.Pets.Common;

public record CreatePetResult(
    Guid Id,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<string> PictureUrls
);
