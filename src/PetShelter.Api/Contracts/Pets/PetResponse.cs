namespace PetShelter.Api.Contracts.Pets;

public record PetResponse(
    string Id,
    string OwnerId,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    string Status,
    List<PetImageResponse> Images
);
