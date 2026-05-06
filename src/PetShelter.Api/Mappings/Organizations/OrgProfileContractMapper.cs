using PetShelter.Api.Common.Models;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Application.Accounts.Common;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;
using PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.Organizations;

[Mapper]
public static partial class OrgProfileMapper
{
    public static OrgProfileResponse ToOrgProfileResponse(this ReturnAppUserDto orgProfile)
    {
        return new OrgProfileResponse(
            orgProfile.Id.ToString(),
            orgProfile.OrgProfile?.OrgName ?? "",
            orgProfile.OrgProfile?.Address ?? "",
            orgProfile.OrgProfile?.Website,
            orgProfile.OrgProfile?.IsVerified ?? false,
            orgProfile.Email,
            orgProfile.PhoneNumber,
            orgProfile.ProfilePictureUrl,
            orgProfile.Role,
            orgProfile.PetsCount,
            null
        );
    }

#pragma warning disable RMG020
    public static partial OrgProfileUpdateCommand ToOrgProfileUpdateCommand(this OrgProfileUpdateRequest updateInfo);
    [MapProperty(nameof(PagedList<ReturnAppUserDto>.Items), nameof(PagedListResponse<OrgProfileResponse>.Items))]
    public static partial PagedListResponse<OrgProfileResponse> ToPagedOrgListResponse(this PagedList<ReturnAppUserDto> pagedList);
    public static partial OrgProfileResponse ToOrgProfileResponse(this MyAccountInfoDto orgProfile);
#pragma warning restore RMG020
}
