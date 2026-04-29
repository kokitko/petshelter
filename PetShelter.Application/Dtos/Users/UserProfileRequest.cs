namespace PetShelter.Application.Dtos.Users;

public record UserProfileRequest(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? AvatarUrl
);