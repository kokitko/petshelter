using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Application.Dtos.Users;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.Users;

[Mapper]
public static partial class UserProfileContractMapper
{
#pragma warning disable RMG020
    [MapProperty(nameof(ReturnAppUserDto.UserProfile.FirstName), nameof(UserProfileResponse.FirstName))]
    [MapProperty(nameof(ReturnAppUserDto.UserProfile.LastName), nameof(UserProfileResponse.LastName))]
    public static partial UserProfileResponse ToUserProfileResponse(this ReturnAppUserDto userProfile);
#pragma warning restore RMG020
}
