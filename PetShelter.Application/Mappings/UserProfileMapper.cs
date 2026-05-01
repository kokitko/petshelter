using Riok.Mapperly.Abstractions;
using PetShelter.Application.Authentication.Commands;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class UserProfileMapper
{
#pragma warning disable RMG012
    [MapperIgnoreTarget(nameof(UserProfile.Id))]
    [MapperIgnoreTarget(nameof(UserProfile.UserId))]
    [MapperIgnoreTarget(nameof(UserProfile.User))]
    public static partial UserProfile ToUserProfile(this UserProfileInfo dto);
#pragma warning restore RMG012
}
