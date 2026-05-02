using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrgProfileQuery;

public class GetOrgProfileQueryHandler(
    IOrgProfileRepository orgProfileRepository
) : IRequestHandler<GetOrgProfileQuery, ErrorOr<ReturnAppUserDto>>
{
    public async Task<ErrorOr<ReturnAppUserDto>> Handle(GetOrgProfileQuery request, CancellationToken cancellationToken)
    {
        var orgProfile = await orgProfileRepository.GetByIdAsync(request.Id);
        if (orgProfile == null)
            return Errors.OrgProfile.NotFound;

        var user = orgProfile.User;

        var result = new ReturnAppUserDto(
            Id: user.Id,
            Email: user.Email,
            PhoneNumber: user.PhoneNumber,
            ProfilePictureUrl: user.ProfilePictureUrl,
            Role: user.Role.ToString(),
            null,
            new ReturnOrgProfileInfo(
                OrgName: orgProfile.OrgName,
                Address: orgProfile.Address,
                Website: orgProfile.Website, 
                IsVerified: orgProfile.IsVerified
            )
        );

        return result;
    }
}
