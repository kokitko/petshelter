using PetShelter.Api.Common.Models;
using PetShelter.Api.Contracts.Pets;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Pets.Commands.CreatePetCommand;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.Pets;

[Mapper]
public static partial class PetContractMapper
{
    [MapProperty(nameof(PagedListResponse<PetResponse>.Items), nameof(PagedList<PetDto>.Items))]
    public static partial PagedListResponse<PetResponse> ToPagedListResponse(this PagedList<PetDto> pagedList);
    [MapProperty(nameof(PetDto.Images), nameof(PetResponse.Images))]
    public static partial PetResponse ToPetResponse(this PetDto pet);
    public static partial PetImageResponse ToPetImageResponse(this PetImageResult petImage);
    [MapProperty(nameof(CreatePetCommand.MainPicture), nameof(CreatePetRequest.MainPicture))]
    [MapProperty(nameof(CreatePetCommand.PicturesToAdd), nameof(CreatePetRequest.PicturesToAdd))]
    public static partial CreatePetCommand ToCreatePetCommand(this CreatePetRequest request);
}
