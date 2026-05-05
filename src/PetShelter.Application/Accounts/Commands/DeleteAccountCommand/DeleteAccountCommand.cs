using ErrorOr;
using MediatR;
using PetShelter.Application.Accounts.Common;

namespace PetShelter.Application.Accounts.Commands.DeleteAccountCommand;

public record DeleteAccountCommand(
    string Password
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;