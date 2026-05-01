using ErrorOr;
using MediatR;
using PetShelter.Application.Authentication.Common;

namespace PetShelter.Application.Authentication.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;
