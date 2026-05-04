using ErrorOr;
using MediatR;
using PetShelter.Application.OrgProfiles.Common;

namespace PetShelter.Application.AdoptionApplications.Queries.GetAdoptionApplicaitonByIdQuery;

public record GetAdoptionApplicationByIdQuery(
    Guid Id
) : IRequest<ErrorOr<ReturnAdoptionApplicationDto>>;