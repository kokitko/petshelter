using Riok.Mapperly.Abstractions;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class AppUserMapper
{
    public static partial ReturnAuthUserDto ToReturnAuthUserDto(this AppUser user);
}
