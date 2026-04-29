using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Authentication.Common;

public record AuthenticationResult(
    string accessToken,
    string refreshToken,
    AppUserResponse User
);
