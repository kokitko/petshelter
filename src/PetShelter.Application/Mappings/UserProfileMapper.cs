using Riok.Mapperly.Abstractions;
using PetShelter.Application.Authentication.Commands;
using PetShelter.Domain.Entities;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class UserProfileMapper
{
#pragma warning disable RMG012
    [MapperIgnoreTarget(nameof(UserProfile.Id))]
    [MapperIgnoreTarget(nameof(UserProfile.UserId))]
    [MapperIgnoreTarget(nameof(UserProfile.User))]
    public static partial UserProfile ToUserProfile(this UserProfileInfo dto);
    public static partial ReturnUserProfileInfo ToReturnUserProfileInfo(this UserProfile userProfile);
#pragma warning restore RMG012
}
