namespace PetShelter.Api.Contracts.Authentication;

public record AuthResponse(
    string AccessToken,
    UserAuthResponse User
);

public record UserAuthResponse(
    Guid Id,
    string Email,
    string? PhoneNumber,
    string Role
);
