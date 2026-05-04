namespace PetShelter.Api.Contracts.OrgProfile;

public record OrgProfileResponse(
    string Id,
    string OrgName,
    string Address,
    string? Website,
    bool isVerified,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role,
    int PetsCount,
    int? ApplicationsCount
);