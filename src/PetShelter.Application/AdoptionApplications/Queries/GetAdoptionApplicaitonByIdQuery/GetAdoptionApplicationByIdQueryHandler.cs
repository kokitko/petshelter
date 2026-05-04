using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.OrgProfiles.Common;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.AdoptionApplications.Queries.GetAdoptionApplicaitonByIdQuery;

public class GetAdoptionApplicationByIdQueryHandler(
    IAdoptionApplicationRepository adoptionApplicationRepository,
    ICurrentUserProvider currentUserProvider
) : IRequestHandler<GetAdoptionApplicationByIdQuery, ErrorOr<ReturnAdoptionApplicationDto>>
{
    public async Task<ErrorOr<ReturnAdoptionApplicationDto>> Handle(GetAdoptionApplicationByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();

        var adoptionApplication = await adoptionApplicationRepository.GetByIdAsync(request.Id);
        if (adoptionApplication == null)
            return Errors.AdoptionApplications.NotFound;

        if (adoptionApplication.ApplicantId != currentUserId && adoptionApplication.Pet.OwnerId != currentUserId)
            return Errors.Authentication.Forbidden;

        return new ReturnAdoptionApplicationDto(
            adoptionApplication.Id,
            adoptionApplication.PetId,
            adoptionApplication.ApplicantId,
            adoptionApplication.Message,
            adoptionApplication.Status
        );
    }
}
