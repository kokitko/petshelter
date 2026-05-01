namespace PetShelter.Api.Contracts.OrgProfile;

public record OrgProfileUpdateResponse(
    string Id,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role,
    OrgProfileUpdateInfo OrgInfo
);

public record OrgProfileUpdateInfo(
    string OrgName,
    string Address,
    string? Website
);


