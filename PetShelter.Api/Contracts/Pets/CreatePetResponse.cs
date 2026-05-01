namespace PetShelter.Api.Contracts.Pets;

public record CreatePetResponse(
    string Id,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<string> PictureUrls
);