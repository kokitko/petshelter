using ErrorOr;
using MediatR;
using PetShelter.Application.Authentication.Common;

namespace PetShelter.Application.Authentication.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    string? PhoneNumber,
    OrgProfileInfo? OrgProfile,
    UserProfileInfo? UserProfile
) : IRequest<ErrorOr<AuthenticationResult>>;

public record UserProfileInfo(
    string FirstName,
    string LastName
);

public record OrgProfileInfo(
    string OrgName,
    string Address,
    string? Website
);