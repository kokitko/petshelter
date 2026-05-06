using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Admin.Commands.AdminDeleteAccountCommand;

public record AdminDeleteAccountCommand(
    string UserId
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;