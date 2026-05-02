namespace PetShelter.Api.Contracts.AdoptionApplication;

public record AdoptionApplicationResponse(
    string Id,
    string PetId,
    string ApplicantId,
    string Message,
    string Status
);
