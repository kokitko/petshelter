using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

public class GetOrganizationsQueryHandler(
    IOrgProfileRepository orgProfileRepository
) : IRequestHandler<GetOrganizationsQuery, ErrorOr<PagedList<ReturnAppUserDto>>>
{
    public async Task<ErrorOr<PagedList<ReturnAppUserDto>>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var (orgProfiles, totalCount) = await orgProfileRepository.GetOrgProfilesAsync(
            request.PageNumber, 
            request.PageSize, 
            request.OrgName, 
            request.Address);

        List<ReturnAppUserDto> orgDtos = orgProfiles.Select(o => new ReturnAppUserDto(
            o.User.Id,
            o.User.Email,
            o.User.PhoneNumber,
            o.User.ProfilePictureUrl,
            o.User.Role.ToString(),
            null,
            new ReturnOrgProfileInfo(
                o.OrgName,
                o.Address,
                o.Website,
                o.IsVerified
            )
        )).ToList();

        var pagedOrgProfiles = new PagedList<ReturnAppUserDto>(
            orgDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return pagedOrgProfiles;
    }
}
