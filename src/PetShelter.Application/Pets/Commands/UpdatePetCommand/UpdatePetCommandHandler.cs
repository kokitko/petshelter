using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Pets.Commands.UpdatePetCommand;

public class UpdatePetCommandHandler(
    IPetRepository petRepository,
    ICurrentUserProvider currentUserProvider,
    IFileStorageService fileStorageService,
    IPetImageRepository petImageRepository,
    ICacheService cacheService
) : IRequestHandler<UpdatePetCommand, ErrorOr<PetDto>>
{
    public async Task<ErrorOr<PetDto>> Handle(UpdatePetCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();

        if (currentUserId == null)
            return Errors.Authentication.Unauthenticated;

        var pet = await petRepository.GetByIdAsync(request.Id);
        if (pet == null)
            return Errors.Pets.NotFound;

        if (currentUserId != pet.OwnerId)
            return Errors.Authentication.Forbidden;

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
                await fileStorageService.DeleteAsync(mainImage.Url);
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

        if (request.PicturesToAdd != null)
        {
            if (pet.Images.Count + request.PicturesToAdd.Count - (request.PictureIdsToRemove?.Count() ?? 0) > 11)
                return Errors.Pets.TooManyImages;

            foreach (var picture in request.PicturesToAdd)
            {
                string imageUrl = await fileStorageService.UploadFileAsync(
                    picture, 
                    $"{pet.Id}_{picture.FileName}", 
                    picture.ContentType);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    await petImageRepository.AddAsync(new PetImage
                    {
                        Id = Guid.NewGuid(),
                        PetId = pet.Id,
                        Url = imageUrl,
                        IsMain = false
                    });
                }
            }
        }

        if (request.PictureIdsToRemove != null)
        {
            foreach (var pictureId in request.PictureIdsToRemove)
            {
                var image = pet.Images.FirstOrDefault(img => img.Id == pictureId);
                if (image != null)
                {
                    await fileStorageService.DeleteAsync(image.Url);
                    await petImageRepository.DeleteAsync(image);
                }
            }
        }

        await petRepository.SaveChangesAsync();
        await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);

        return pet.ToPetDto();
    }
}
