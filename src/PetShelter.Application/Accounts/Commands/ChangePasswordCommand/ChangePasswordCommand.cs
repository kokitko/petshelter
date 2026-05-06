using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Accounts.Commands.ChangePasswordCommand;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;