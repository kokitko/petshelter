using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Commands.CreatePetCommand;

public record CreatePetCommand(
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    IFormFile MainPicture,
    List<IFormFile>? PicturesToAdd
) : IRequest<ErrorOr<PetDto>>;
