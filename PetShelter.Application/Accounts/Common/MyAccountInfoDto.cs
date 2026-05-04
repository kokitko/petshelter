namespace PetShelter.Application.Accounts.Common;

public record MyAccountInfoDto(
    Guid Id,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role,
    int PetsCount,
    int ApplicationsCount,
    string? FirstName,
    string? LastName,
    string? OrgName,
    string? Address,
    string? Website,
    bool? IsVerified
);