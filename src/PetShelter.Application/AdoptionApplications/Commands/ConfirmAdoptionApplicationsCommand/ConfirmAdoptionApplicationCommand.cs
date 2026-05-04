using ErrorOr;
using MediatR;

namespace PetShelter.Application.AdoptionApplications.Commands.ConfirmAdoptionApplicationsCommand;

public record ConfirmAdoptionApplicationCommand(
    Guid Id
) : IRequest<ErrorOr<bool>>;