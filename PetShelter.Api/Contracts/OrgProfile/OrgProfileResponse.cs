namespace PetShelter.Api.Contracts.OrgProfile;

public record OrgProfileResponse(
    string Id,
    string OrgName,
    string Address,
    string? Website,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role
);