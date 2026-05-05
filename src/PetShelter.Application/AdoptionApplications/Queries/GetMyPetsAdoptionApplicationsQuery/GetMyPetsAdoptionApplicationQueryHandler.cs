using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Mappings;
using PetShelter.Application.OrgProfiles.Common;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Queries.GetMyPetsAdoptionApplicationsQuery;

public class GetMyPetsAdoptionApplicationQueryHandler(
    IAdoptionApplicationRepository adoptionApplicationRepository,
    ICurrentUserProvider currentUserProvider
) : IRequestHandler<GetMyPetsAdoptionApplicationQuery, ErrorOr<PagedList<ReturnAdoptionApplicationDto>>>
{
    public async Task<ErrorOr<PagedList<ReturnAdoptionApplicationDto>>> Handle(GetMyPetsAdoptionApplicationQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var (applications, totalCount) = await adoptionApplicationRepository.GetByOwnerIdAsync(
            userId.Value,
            Enum.TryParse<ApplicationStatus>(request.Status, true, out var status) ? status : null,
            request.PageNumber,
            request.PageSize
        );

        var applicationDtos = applications.Select(app => app.ToReturnAdoptionApplicationDto()).ToList();

        return new PagedList<ReturnAdoptionApplicationDto>(applicationDtos, totalCount, request.PageNumber, request.PageSize);
    }
}
