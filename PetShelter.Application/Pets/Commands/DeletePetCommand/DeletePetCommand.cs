using ErrorOr;
using MediatR;

namespace PetShelter.Application.Pets.Commands.DeletePetCommand;

public record DeletePetCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;