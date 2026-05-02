namespace PetShelter.Api.Contracts.Pets;

public record UpdatePetRequest(
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    IFormFile? MainPicture,
    List<IFormFile>? PicturesToAdd,
    List<Guid>? PictureIdsToRemove
);