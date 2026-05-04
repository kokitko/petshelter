using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Authentication.Common;

public record AuthenticationResult(
    string AccessToken,
    string RefreshToken,
    ReturnAuthUserDto User
);
