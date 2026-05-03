using ErrorOr;
using MediatR;

namespace PetShelter.Application.Pets.Commands.ConfirmPetCommand;

public record ConfirmPetCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;