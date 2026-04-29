using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Application.Pets.Queries;

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
    }
}
