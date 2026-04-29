using PetShelter.Domain.Entities;
using PetShelter.Application.Dtos.Pet;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Application.Mappings;

[Mapper]
public static partial class PetMapper
{
    [MapProperty("Owner.Email", nameof(PetResponse.OwnerName))]
    [MapProperty(nameof(Pet.Images), nameof(PetResponse.ImageUrls))]
    public static partial PetResponse ToPetResponse(this Pet pet);
    public static partial IQueryable<PetResponse> ProjectToPetResponse(this IQueryable<Pet> q);
    private static string MapImageToString(PetImage image) => image.Url;
}
