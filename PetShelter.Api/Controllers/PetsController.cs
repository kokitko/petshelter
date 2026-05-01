using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Application.Pets.Queries;
using PetShelter.Api.Contracts.Pets;
using PetShelter.Application.Pets.Commands.CreatePetCommand;

namespace PetShelter.Api.Controllers
{
    [Route("api/pets")]
    public class PetsController(ISender sender) : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetPetsPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? species = null)
        {
            var query = new GetPetsPagedQuery(pageNumber, pageSize, species);
            var result = await sender.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePet([FromForm] CreatePetRequest request)
        {
            var command = new CreatePetCommand(
                request.Name,
                request.Species,
                request.Breed,
                request.Age,
                request.Description,
                request.Picture
            );

            var result = await sender.Send(command);
            return result.Match(
                pet => Ok(new CreatePetResponse(
                    pet.Id.ToString(),
                    pet.Name,
                    pet.Species,
                    pet.Breed,
                    pet.Age,
                    pet.Description,
                    pet.PictureUrls
                )),
                error => Problem(error)
            );
        }
    }
}
