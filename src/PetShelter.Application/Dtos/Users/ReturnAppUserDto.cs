namespace PetShelter.Application.Dtos.Users;

public record ReturnAppUserDto(
    Guid Id,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role,
    int PetsCount,
    ReturnUserProfileInfo? UserProfile,
    ReturnOrgProfileInfo? OrgProfile
);

public record ReturnUserProfileInfo(
    string FirstName,
    string LastName
);

public record ReturnOrgProfileInfo(
    string OrgName,
    string Address,
    string? Website,
    bool IsVerified
);
