namespace PetShelter.Api.Contracts.AdoptionApplication;

public record CreateAdoptionApplicationRequest(
    string PetId,
    string Message
);
