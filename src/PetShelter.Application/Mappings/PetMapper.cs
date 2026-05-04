using PetShelter.Domain.Entities;
using PetShelter.Application.Dtos.Pet;
using Riok.Mapperly.Abstractions;
using PetShelter.Application.Pets.Commands.CreatePetCommand;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class PetMapper
{
#pragma warning disable RMG012
    [MapperIgnoreTarget(nameof(Pet.Id))]
    [MapperIgnoreTarget(nameof(Pet.Owner))]
    [MapperIgnoreTarget(nameof(Pet.Images))]
    [MapperIgnoreTarget(nameof(Pet.CreatedAt))]
    [MapperIgnoreTarget(nameof(Pet.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Pet.Applications))]
    public static partial Pet ToPet(this CreatePetCommand request);
    [MapProperty(nameof(Pet.Images), nameof(PetDto.Images))]
    public static partial PetDto ToPetDto(this Pet pet);
    public static partial IQueryable<PetDto> ToPetDto(this IQueryable<Pet> q);
    private static PetImageResult PetImageToPetImageResult(PetImage petImage) => new PetImageResult(petImage.Id, petImage.IsMain, petImage.Url);
#pragma warning restore RMG012
}