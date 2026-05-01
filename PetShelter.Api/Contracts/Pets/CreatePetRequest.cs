namespace PetShelter.Api.Contracts.Pets;

public record CreatePetRequest(
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<IFormFile> Picture
);
