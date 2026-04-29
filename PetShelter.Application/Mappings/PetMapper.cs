using PetShelter.Domain.Entities;
using PetShelter.Application.Dtos.Pet;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class PetMapper
{
    [MapProperty("Owner.Email", nameof(ReturnPetDto.OwnerName))]
    [MapProperty(nameof(Pet.Images), nameof(ReturnPetDto.ImageUrls))]
    public static partial ReturnPetDto ToReturnPetDto(this Pet pet);
    public static partial IQueryable<ReturnPetDto> ProjectToReturnPetDto(this IQueryable<Pet> q);
    
    private static string MapImageToString(PetImage image) => image.Url;
}
