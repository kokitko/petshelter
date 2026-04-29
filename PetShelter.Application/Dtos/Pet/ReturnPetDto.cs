namespace PetShelter.Application.Dtos.Pet;

public record PetResponse(
    Guid Id,
    string Name,
    string Breed,
    string Species,
    int Age,
    string OwnerName,
    List<string> ImageUrls
);
