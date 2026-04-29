namespace PetShelter.Application.Dtos.Users;

public record OrgProfileRequest(
    string OrgName,
    string Address,
    string? Website
);
