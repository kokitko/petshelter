using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Accounts.Commands.DeleteAccountCommand;

public record DeleteAccountCommand(
    string Password
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;