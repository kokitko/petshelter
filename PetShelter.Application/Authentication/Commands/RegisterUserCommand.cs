using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Authentication.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    OrgProfileRequest? OrgProfile,
    UserProfileRequest? UserProfile
);
