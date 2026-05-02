using PetShelter.Domain.Entities;
using PetShelter.Application.Dtos.Pet;
using Riok.Mapperly.Abstractions;
using PetShelter.Application.Pets.Commands.CreatePetCommand;
using PetShelter.Application.Pets.Common;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class PetMapper
{
#pragma warning disable RMG012
    [MapProperty("Owner.Email", nameof(PetResponse.OwnerName))]
    [MapProperty(nameof(Pet.Images), nameof(PetResponse.ImageUrls))]
    public static partial PetResponse ToPetResponse(this Pet pet);
    [MapperIgnoreTarget(nameof(Pet.Id))]
    [MapperIgnoreTarget(nameof(Pet.Owner))]
    [MapperIgnoreTarget(nameof(Pet.Images))]
    [MapperIgnoreTarget(nameof(Pet.CreatedAt))]
    [MapperIgnoreTarget(nameof(Pet.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Pet.Applications))]
    public static partial Pet ToPet(this CreatePetCommand request);
    [MapProperty(nameof(Pet.Images), nameof(PetResult.PicturesInfo))]
    public static partial PetResult ToPetResult(this Pet pet);
    private static PetImageResult PetImageToPetImageResult(PetImage petImage) => new PetImageResult(petImage.Id, petImage.IsMain, petImage.Url);
    public static partial IQueryable<PetResponse> ProjectToPetResponse(this IQueryable<Pet> q);
#pragma warning restore RMG012
}