using ErrorOr;
using MediatR;

namespace PetShelter.Application.Accounts.Commands.ChangePasswordCommand;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword
) : IRequest<ErrorOr<bool>>;