using MediatR;
using ErrorOr;
using PetShelter.Application.Authentication.Common;

namespace PetShelter.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<ErrorOr<AuthenticationResult>>;