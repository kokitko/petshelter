namespace PetShelter.Api.Contracts.UserProfile;

public record UserProfileUpdateRequest(
    string Email,
    string? PhoneNumber,
    string FirstName,
    string LastName,
    IFormFile? ProfilePicture
);