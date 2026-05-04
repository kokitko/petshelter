using ErrorOr;
using MediatR;

namespace PetShelter.Application.AdoptionApplications.Commands.RejectAdoptionApplicationCommand;

public record RejectAdoptionApplicationCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;