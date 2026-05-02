using MediatR;
using ErrorOr;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Domain.Common.Errors;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Entities;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Commands.CreatePetCommand;

public class CreatePetCommandHandler(
    IPetRepository petRepository,
    IFileStorageService fileStorageService,
    ICurrentUserProvider currentUserProvider
) : IRequestHandler<CreatePetCommand, ErrorOr<PetDto>>
{
    public async Task<ErrorOr<PetDto>> Handle(CreatePetCommand request, CancellationToken cancellationToken)
    {
        string userId = currentUserProvider.GetCurrentUserId()?.ToString() ?? string.Empty;
        if (string.IsNullOrEmpty(userId))
            return Errors.Authentication.Unauthenticated;

        Pet pet = request.ToPet();
        pet.Id = Guid.NewGuid();
        pet.OwnerId = Guid.Parse(userId);

        pet.Images = new List<PetImage>();

        string mainImageUrl = await fileStorageService.UploadFileAsync(
            request.MainPicture, 
            $"{pet.Id}_{request.MainPicture.FileName}", 
            request.MainPicture.ContentType);

        pet.Images.Add(new PetImage
        {
            Id = Guid.NewGuid(),
            PetId = pet.Id,
            Url = mainImageUrl,
            IsMain = true
        });

        if (request.PicturesToAdd != null)
            foreach (var picture in request.PicturesToAdd)
            {
                string imageUrl = await fileStorageService.UploadFileAsync(
                    picture, 
                    $"{pet.Id}_{picture.FileName}", 
                    picture.ContentType);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    pet.Images.Add(new PetImage
                    {
                        Id = Guid.NewGuid(),
                        PetId = pet.Id,
                        Url = imageUrl,
                        IsMain = false
                    });
                }
            }

        await petRepository.AddAsync(pet);
        return pet.ToPetDto();
    }
}
