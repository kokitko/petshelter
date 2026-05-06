using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Accounts.Commands.ChangeEmailCommand;

public record ChangeEmailCommand(
    string NewEmail,
    string CurrentPassword
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;