using ErrorOr;
using MediatR;
using PetShelter.Application.OrgProfiles.Common;

namespace PetShelter.Application.AdoptionApplications.Commands.CreateAdoptionApplicationCommand;

public record CreateAdoptionApplicationCommand(
    Guid PetId,
    string Message
) : IRequest<ErrorOr<ReturnAdoptionApplicationDto>>;
