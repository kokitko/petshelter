using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Pets.Commands.DeletePetCommand;

public record DeletePetCommand(
    Guid Id
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;