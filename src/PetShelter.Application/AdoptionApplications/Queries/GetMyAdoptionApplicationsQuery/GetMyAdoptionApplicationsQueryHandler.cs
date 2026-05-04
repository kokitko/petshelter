using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Mappings;
using PetShelter.Application.OrgProfiles.Common;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Queries.GetMyAdoptionApplicationsQuery;

public class GetMyAdoptionApplicationsQueryHandler(
    IAdoptionApplicationRepository adoptionApplicationRepository,
    ICurrentUserProvider currentUserProvider
) : IRequestHandler<GetMyAdoptionApplicationsQuery, ErrorOr<PagedList<ReturnAdoptionApplicationDto>>>
{
    public async Task<ErrorOr<PagedList<ReturnAdoptionApplicationDto>>> Handle(GetMyAdoptionApplicationsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();
        if (currentUserId == null)
            return Errors.Authentication.Unauthenticated;

        var (applications, totalCount) = await adoptionApplicationRepository.GetByApplicantIdAsync(
            currentUserId.Value, 
            Enum.TryParse<ApplicationStatus>(request.Status, out var status) ? status : null,
            request.PageNumber, 
            request.PageSize);

        var applicationDtos = applications.Select(app => app.ToReturnAdoptionApplicationDto()).ToList();

        return new PagedList<ReturnAdoptionApplicationDto>(applicationDtos, totalCount, request.PageNumber, request.PageSize);
    }
}
