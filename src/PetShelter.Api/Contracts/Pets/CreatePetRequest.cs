namespace PetShelter.Api.Contracts.Pets;

public record CreatePetRequest(
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    IFormFile MainPicture,
    List<IFormFile>? PicturesToAdd
);
