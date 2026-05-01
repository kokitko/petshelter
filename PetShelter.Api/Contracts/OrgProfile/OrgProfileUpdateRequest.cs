namespace PetShelter.Api.Contracts.OrgProfile;

public record OrgProfileUpdateRequest(
    string Email,
    string? PhoneNumber,
    string OrgName,
    string Address,
    string? Website,
    IFormFile? ProfilePicture
);