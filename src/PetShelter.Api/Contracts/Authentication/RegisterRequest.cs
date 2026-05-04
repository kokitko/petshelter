namespace PetShelter.Api.Contracts.Authentication;

public record RegisterRequest(
    string Email,
    string Password,
    string? PhoneNumber,
    OrgProfileRequest? OrgProfile,
    UserProfileRequest? UserProfile
);

public record OrgProfileRequest(
    string OrgName,
    string Address,
    string? Website
);

public record UserProfileRequest(
    string FirstName,
    string LastName
);
