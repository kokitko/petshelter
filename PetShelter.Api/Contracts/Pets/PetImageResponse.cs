namespace PetShelter.Api.Contracts.Pets;

public record PetImageResponse(
    string Id,
    bool IsMain,
    string Url
);