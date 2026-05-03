using PetShelter.Api.Common.Models;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;
using PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.Organizations;

[Mapper]
public static partial class OrgProfileMapper
{
#pragma warning disable RMG020
    [MapProperty(nameof(ReturnAppUserDto.OrgProfile.OrgName), nameof(OrgProfileResponse.OrgName))]
    [MapProperty(nameof(ReturnAppUserDto.OrgProfile.Address), nameof(OrgProfileResponse.Address))]
    [MapProperty(nameof(ReturnAppUserDto.OrgProfile.Website), nameof(OrgProfileResponse.Website))]
    [MapProperty(nameof(ReturnAppUserDto.OrgProfile.IsVerified), nameof(OrgProfileResponse.isVerified))]
    public static partial OrgProfileResponse ToOrgProfileResponse(this ReturnAppUserDto orgProfile);
    public static partial OrgProfileUpdateCommand ToOrgProfileUpdateCommand(this OrgProfileUpdateRequest updateInfo);
    [MapProperty(nameof(PagedList<ReturnAppUserDto>.Items), nameof(PagedListResponse<OrgProfileResponse>.Items))]
    public static partial PagedListResponse<OrgProfileResponse> ToPagedListResponse(this PagedList<ReturnAppUserDto> pagedList);
#pragma warning restore RMG020
}
