using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Application.Accounts.Common;
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
#pragma warning restore RMG020
}
