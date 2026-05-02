namespace PetShelter.Application.Dtos.Pet;

public record PetImageResult(
    Guid Id,
    bool IsMain,
    string Url
);