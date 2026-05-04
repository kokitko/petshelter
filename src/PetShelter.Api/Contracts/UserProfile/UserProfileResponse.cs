namespace PetShelter.Api.Contracts.UserProfile;

public record UserProfileResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? ProfilePictureUrl,
    string Role,
    int PetsCount,
    int? ApplicationsCount
);
