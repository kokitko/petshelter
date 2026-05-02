using ErrorOr;
using MediatR;

namespace PetShelter.Application.Accounts.Commands.DeleteAccountCommand;

public record DeleteAccountCommand(
    string Password
) : IRequest<ErrorOr<bool>>;