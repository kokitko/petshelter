using MediatR;
using ErrorOr;
using PetShelter.Application.Pets.Common;
using Microsoft.AspNetCore.Http;

namespace PetShelter.Application.Pets.Commands.CreatePetCommand;

public record CreatePetCommand(
    string Name,
    string Species,
    string Breed,
    int Age,
    string Description,
    List<IFormFile> Picture
) : IRequest<ErrorOr<CreatePetResult>>;
