using ErrorOr;
using MediatR;

namespace PetShelter.Application.Authentication.Commands.Logout;

public record LogoutCommand(
    string RefreshToken
) : IRequest<ErrorOr<bool>>;