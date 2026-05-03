using PetShelter.Application.OrgProfiles.Common;
using PetShelter.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class AdoptionApplicationMapper
{
    public static partial ReturnAdoptionApplicationDto ToReturnAdoptionApplicationDto(this AdoptionApplication application);
}
