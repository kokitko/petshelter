using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Api.Contracts.Pets;
using PetShelter.Application.Pets.Commands.UpdatePetCommand;
using PetShelter.Application.Pets.Queries.GetUserPetsQuery;
using PetShelter.Application.Pets.Queries.GetPetByIdQuery;
using PetShelter.Application.Pets.Queries.GetPetsQuery;
using PetShelter.Api.Mappings.Pets;

namespace PetShelter.Api.Controllers
{
    [Route("api/pets")]
    public class PetsController(ISender sender) : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetById(Guid id)
        {
            var query = new GetPetByIdQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(success.ToPetResponse()),
                error => Problem(error)
            );
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPets(
            Guid userId,
            [FromQuery] int? age,
            [FromQuery] string? name, 
            [FromQuery] string? species, 
            [FromQuery] string? breed,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetUserPetsQuery(userId, name, species, breed, age, page, pageSize);
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(success.ToPagedListResponse()),
                error => Problem(error)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetPetsPaged(
            [FromQuery] int? age,
            [FromQuery] string? name, 
            [FromQuery] string? species, 
            [FromQuery] string? breed,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetPetsQuery(name, species, breed, age, page, pageSize);
            var result = await sender.Send(query);

            return result.Match(
                success => Ok(success.ToPagedListResponse()),
                error => Problem(error)
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreatePet([FromForm] CreatePetRequest request)
        {
            var command = request.ToCreatePetCommand();

            var result = await sender.Send(command);
            return result.Match(
                pet => Ok(pet.ToPetResponse()),
                error => Problem(error)
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(Guid id, [FromForm] UpdatePetRequest request)
        {
            var command = new UpdatePetCommand(
                id,
                request.Name,
                request.Species,
                request.Breed,
                request.Age,
                request.Description,
                request.MainPicture,
                request.PicturesToAdd,
                request.PictureIdsToRemove
            );

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(success.ToPetResponse()),
                error => Problem(error)
            );
        }
    }
}
