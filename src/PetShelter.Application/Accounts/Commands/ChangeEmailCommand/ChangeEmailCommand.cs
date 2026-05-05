using ErrorOr;
using MediatR;
using PetShelter.Application.Accounts.Common;

namespace PetShelter.Application.Accounts.Commands.ChangeEmailCommand;

public record ChangeEmailCommand(
    string NewEmail,
    string CurrentPassword
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;