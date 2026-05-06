using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Admin.Commands.AdminUpdatePetCommand;

public record AdminUpdatePetCommand(
    string PetId,
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    IFormFile? MainPicture,
    List<IFormFile>? PicturesToAdd,
    List<Guid>? PictureIdsToRemove
) : IRequest<ErrorOr<PetDto>>;