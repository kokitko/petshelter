using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Admin.Commands.AdminDeletePetCommand;

public record AdminDeletePetCommand(
    string PetId
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;