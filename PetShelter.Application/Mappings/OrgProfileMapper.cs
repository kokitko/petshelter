using Riok.Mapperly.Abstractions;
using PetShelter.Domain.Entities;
using PetShelter.Application.Authentication.Commands;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class OrgProfileMapper
{
#pragma warning disable RMG012
    [MapperIgnoreTarget(nameof(OrgProfile.UserId))]
    [MapperIgnoreTarget(nameof(OrgProfile.User))]
    [MapperIgnoreTarget(nameof(OrgProfile.IsVerified))]
    public static partial OrgProfile ToOrgProfile(this OrgProfileInfo dto);
#pragma warning restore RMG012
}
