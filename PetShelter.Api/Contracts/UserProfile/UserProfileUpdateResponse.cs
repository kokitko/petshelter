namespace PetShelter.Api.Contracts.UserProfile;

public record UserProfileUpdateResponse(
    string Id,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role,
    UserProfileUpdateInfo UserInfo
);

public record UserProfileUpdateInfo(
    string FirstName,
    string LastName
);
