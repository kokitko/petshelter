using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Application.Pets.Queries;
using PetShelter.Api.Contracts.Pets;
using PetShelter.Application.Pets.Commands.CreatePetCommand;
using PetShelter.Application.Pets.Commands.UpdatePetCommand;
using PetShelter.Application.Pets.Queries.GetUserPetsQuery;
using PetShelter.Api.Common.Models;

namespace PetShelter.Api.Controllers
{
    [Route("api/pets")]
    public class PetsController(ISender sender) : ApiController
    {
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
                success => Ok(
                    new PagedListResponse<PetResponse>(
                        success.Items.Select(p => new PetResponse(
                            p.Id.ToString(),
                            p.OwnerId.ToString(),
                            p.Name,
                            p.Species,
                            p.Breed,
                            p.Age,
                            p.Description,
                            p.Images.Select(url => new PetImageResponse(
                                url.Id.ToString(),
                                url.IsMain,
                                url.Url
                            )).ToList()
                )).ToList(), 
                success.PageNumber, 
                success.TotalPages, 
                success.TotalCount, 
                success.HasPreviousPage, 
                success.HasNextPage)
                ),
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
            var query = new GetPetsPagedQuery(name, species, breed, age, page, pageSize);
            var result = await sender.Send(query);
            // TODO: map to paged list response
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
                request.MainPicture,
                request.PicturesToAdd
            );

            var result = await sender.Send(command);
            return result.Match(
                pet => Ok(new PetResponse(
                    pet.Id.ToString(),
                    pet.OwnerId.ToString(),
                    pet.Name,
                    pet.Species,
                    pet.Breed,
                    pet.Age,
                    pet.Description,
                    pet.Images.Select(url => new PetImageResponse(
                        url.Id.ToString(),
                        url.IsMain,
                        url.Url
                    )).ToList()
                )),
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
                pet => Ok(new PetResponse(
                    pet.Id.ToString(),
                    pet.OwnerId.ToString(),
                    pet.Name,
                    pet.Species,
                    pet.Breed,
                    pet.Age,
                    pet.Description,
                    pet.Images.Select(img => new PetImageResponse(
                        img.Id.ToString(),
                        img.IsMain,
                        img.Url
                    )).ToList()
                )),
                error => Problem(error)
            );
        }
    }
}
