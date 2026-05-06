using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Admin.Commands.AdminUpdatePetCommand;

public class AdminUpdatePetCommandHandler(
    IPetRepository petRepository,
    IPetImageRepository petImageRepository,
    IFileStorageService fileStorageService,
    ICacheService cacheService
) : IRequestHandler<AdminUpdatePetCommand, ErrorOr<PetDto>>
{
    public async Task<ErrorOr<PetDto>> Handle(AdminUpdatePetCommand request, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetByIdAsync(Guid.Parse(request.PetId));
        if (pet == null)
            return Errors.Pets.NotFound;

        pet.Name = request.Name;
        pet.Species = request.Species;
        pet.Breed = request.Breed;
        pet.Age = request.Age;
        pet.Description = request.Description;

        if (request.MainPicture != null)
        {
            var mainImage = pet.Images.FirstOrDefault(img => img.IsMain);
            if (mainImage != null)
            {
                await petImageRepository.DeleteAsync(mainImage);
            }

            string mainImageUrl = await fileStorageService.UploadFileAsync(
                request.MainPicture, 
                $"{pet.Id}_{request.MainPicture.FileName}", 
                request.MainPicture.ContentType);

            await petImageRepository.AddAsync(new PetImage
            {
                Id = Guid.NewGuid(),
                PetId = pet.Id,
                Url = mainImageUrl,
                IsMain = true
            });
        }

        await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);
        return pet.ToPetDto();
    }
}
