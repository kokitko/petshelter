using ErrorOr;
using MediatR;
using PetShelter.Application.Pets.Common;

namespace PetShelter.Application.Pets.Commands.DeletePetCommand;

public record DeletePetCommand(
    Guid Id
) : IRequest<ErrorOr<DeletePetDto>>;