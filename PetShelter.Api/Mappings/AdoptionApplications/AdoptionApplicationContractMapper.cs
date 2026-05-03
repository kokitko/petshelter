using PetShelter.Api.Contracts.AdoptionApplication;
using PetShelter.Application.OrgProfiles.Common;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.AdoptionApplications;

[Mapper]
public static partial class AdoptionApplicationContractMapper
{
    public static partial AdoptionApplicationResponse ToAdoptionApplicationResponse(this ReturnAdoptionApplicationDto application);
}