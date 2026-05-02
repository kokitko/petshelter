namespace PetShelter.Application.Pets.Common;

public record PetImageResult(
    Guid Id,
    bool IsMain,
    string Url
);
