using ErrorOr;
using MediatR;

namespace PetShelter.Application.Accounts.Commands.ChangeEmailCommand;

public record ChangeEmailCommand(
    string NewEmail,
    string CurrentPassword
) : IRequest<ErrorOr<bool>>;