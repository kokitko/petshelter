using Riok.Mapperly.Abstractions;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class AppUserMapper
{
#pragma warning disable RMG004
    public static partial ReturnAuthUserDto ToReturnAuthUserDto(this AppUser user);
    [MapperIgnoreTarget(nameof(AppUser.PasswordHash))]
    [MapperIgnoreTarget(nameof(AppUser.Pets))]
    [MapperIgnoreTarget(nameof(AppUser.Applications))]
    [MapperIgnoreTarget(nameof(AppUser.RefreshTokens))]
    public static partial ReturnAppUserDto ToReturnAppUserDto(this AppUser user);
#pragma warning restore RMG004
}
