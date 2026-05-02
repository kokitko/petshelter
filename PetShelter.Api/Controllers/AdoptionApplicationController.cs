using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.AdoptionApplication;
using PetShelter.Application.AdoptionApplications.Commands.CreateAdoptionApplicationCommand;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdoptionApplicationController(ISender sender) : ApiController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdoptionApplication(CreateAdoptionApplicationRequest request)
        {
            var command = new CreateAdoptionApplicationCommand(
                Guid.Parse(request.PetId),
                request.Message
            );

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(
                    new AdoptionApplicationResponse(
                        success.Id.ToString(),
                        success.PetId.ToString(),
                        success.ApplicantId.ToString(),
                        success.Message,
                        success.Status.ToString()
                    )
                ),
                error => Problem(error)
            );
        }
    }
}
