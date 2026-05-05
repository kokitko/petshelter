using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Api.Contracts.Pets;
using PetShelter.Application.Pets.Commands.UpdatePetCommand;
using PetShelter.Application.Pets.Queries.GetUserPetsQuery;
using PetShelter.Application.Pets.Queries.GetPetByIdQuery;
using PetShelter.Application.Pets.Queries.GetPetsQuery;
using PetShelter.Api.Mappings.Pets;
using PetShelter.Application.Pets.Commands.ConfirmPetCommand;
using PetShelter.Application.Pets.Commands.DeletePetCommand;
using Microsoft.AspNetCore.Authorization;

namespace PetShelter.Api.Controllers
{
    [Route("api/pets")]
    public class PetsController(
        ISender sender,
        ILogger<PetsController> logger) : ApiController(logger)
    {
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePet(Guid id)
        {
            logger.LogInformation("DELETE /api/pets/{Id} called", id);
            var command = new DeletePetCommand(id);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("DELETE /api/pets/[id] successful for petId: {PetId}", success.PetId);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmPet(Guid id)
        {
            logger.LogInformation("PUT /api/pets/{Id}/confirm called", id);
            var command = new ConfirmPetCommand(id);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("PUT /api/pets/[id]/confirm successful for petId: {PetId}", id);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPetById(Guid id)
        {
            logger.LogInformation("GET /api/pets/{Id} called", id);
            var query = new GetPetByIdQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/pets/[id] successful for petId: {PetId}", id);
                    return Ok(success.ToPetResponse());
                },
                error => Problem(error)
            );
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserPets(
            Guid userId,
            [FromQuery] string? status,
            [FromQuery] int? age,
            [FromQuery] string? name, 
            [FromQuery] string? species, 
            [FromQuery] string? breed,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            logger.LogInformation("GET /api/pets/user/{UserId} called with status: {Status}, age: {Age}, name: {Name}, species: {Species}, breed: {Breed}, pageNumber: {PageNumber}, pageSize: {PageSize}", 
                userId, status, age, name, species, breed, page, pageSize);
            var query = new GetUserPetsQuery(userId, status, name, species, breed, age, page, pageSize);
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/pets/user/[userId] successful for userId: {UserId}", userId);
                    return Ok(success.ToPagedListResponse());
                },
                error => Problem(error)
            );
        }
        [HttpGet]
        public async Task<IActionResult> GetPetsPaged(
            [FromQuery] string? status,
            [FromQuery] int? age,
            [FromQuery] string? name, 
            [FromQuery] string? species, 
            [FromQuery] string? breed,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            logger.LogInformation("GET /api/pets called with status: {Status}, age: {Age}, name: {Name}, species: {Species}, breed: {Breed}, pageNumber: {PageNumber}, pageSize: {PageSize}", 
                status, age, name, species, breed, page, pageSize);
            var query = new GetPetsQuery(status, name, species, breed, age, page, pageSize);
            var result = await sender.Send(query);

            return result.Match(
                success => {
                    logger.LogInformation("GET /api/pets successful with {Count} results", success.TotalCount);
                    return Ok(success.ToPagedListResponse());
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePet([FromForm] CreatePetRequest request)
        {
            logger.LogInformation("POST /api/pets called with name: {Name}, species: {Species}, breed: {Breed}, age: {Age}", request.Name, request.Species, request.Breed, request.Age);
            var command = request.ToCreatePetCommand();

            var result = await sender.Send(command);
            return result.Match(
                pet => {
                    logger.LogInformation("POST /api/pets successful for petId: {PetId}", pet.Id);
                    return Ok(pet.ToPetResponse());
                },
                error => Problem(error)
            );
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePet(Guid id, [FromForm] UpdatePetRequest request)
        {
            logger.LogInformation("PUT /api/pets/{Id} called with name: {Name}, species: {Species}, breed: {Breed}, age: {Age}", id, request.Name, request.Species, request.Breed, request.Age);
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
                success => {
                    logger.LogInformation("PUT /api/pets/[id] successful for petId: {PetId}", id);
                    return Ok(success.ToPetResponse());
                },
                error => Problem(error)
            );
        }
    }
}
