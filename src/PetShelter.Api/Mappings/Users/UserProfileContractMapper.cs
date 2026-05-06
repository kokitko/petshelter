using PetShelter.Api.Common.Models;
using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Application.Accounts.Common;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.Users;

[Mapper]
public static partial class UserProfileContractMapper
{
    public static UserProfileResponse ToUserProfileResponse(this ReturnAppUserDto userProfile)
    {
        return new UserProfileResponse(
            userProfile.Id.ToString(),
            userProfile.UserProfile?.FirstName ?? "",
            userProfile.UserProfile?.LastName ?? "",
            userProfile.Email,
            userProfile.PhoneNumber,
            userProfile.ProfilePictureUrl,
            userProfile.Role,
            userProfile.PetsCount,
            null
        );
    }

#pragma warning disable RMG020
    public static partial UserProfileResponse ToUserProfileResponse(this MyAccountInfoDto userProfile);
        [MapProperty(nameof(PagedList<ReturnAppUserDto>.Items), nameof(PagedListResponse<UserProfileResponse>.Items))]
    public static partial PagedListResponse<UserProfileResponse> ToPagedUserListResponse(this PagedList<ReturnAppUserDto> pagedList);
#pragma warning restore RMG020
}
