using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.AdoptionApplication;
using PetShelter.Api.Mappings.AdoptionApplications;
using PetShelter.Application.AdoptionApplications.Commands.ConfirmAdoptionApplicationsCommand;
using PetShelter.Application.AdoptionApplications.Commands.CreateAdoptionApplicationCommand;
using PetShelter.Application.AdoptionApplications.Commands.RejectAdoptionApplicationCommand;
using PetShelter.Application.AdoptionApplications.Queries.GetAdoptionApplicaitonByIdQuery;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdoptionApplicationController(ISender sender) : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdoptionApplicationById(Guid id)
        {
            var query = new GetAdoptionApplicationByIdQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(success.ToAdoptionApplicationResponse()),
                error => Problem(error)
            );
        }
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectAdoptionApplication(Guid id)
        {
            var command = new RejectAdoptionApplicationCommand(id);
            var result = await sender.Send(command);
            return result.Match(
                success => Ok(success),
                error => Problem(error)
            );
        }

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmAdoptionApplication(Guid id)
        {
            var command = new ConfirmAdoptionApplicationCommand(id);
            var result = await sender.Send(command);
            return result.Match(
                success => Ok(success),
                error => Problem(error)
            );
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdoptionApplication(CreateAdoptionApplicationRequest request)
        {
            var command = new CreateAdoptionApplicationCommand(
                Guid.Parse(request.PetId),
                request.Message
            );

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(success.ToAdoptionApplicationResponse()),
                error => Problem(error)
            );
        }
    }
}
