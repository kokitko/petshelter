using PetShelter.Api.Common.Models;
using PetShelter.Api.Contracts.AdoptionApplication;
using PetShelter.Application.Common.Models;
using PetShelter.Application.OrgProfiles.Common;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.AdoptionApplications;

[Mapper]
public static partial class AdoptionApplicationContractMapper
{
    public static partial AdoptionApplicationResponse ToAdoptionApplicationResponse(this ReturnAdoptionApplicationDto application);
    public static partial PagedListResponse<AdoptionApplicationResponse> ToPagedListResponse(this PagedList<ReturnAdoptionApplicationDto> pagedList);
}