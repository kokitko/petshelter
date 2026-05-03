using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.AdoptionApplication;
using PetShelter.Api.Mappings.AdoptionApplications;
using PetShelter.Application.AdoptionApplications.Commands.ConfirmAdoptionApplicationsCommand;
using PetShelter.Application.AdoptionApplications.Commands.CreateAdoptionApplicationCommand;
using PetShelter.Application.AdoptionApplications.Commands.RejectAdoptionApplicationCommand;
using PetShelter.Application.AdoptionApplications.Queries.GetAdoptionApplicaitonByIdQuery;
using PetShelter.Application.AdoptionApplications.Queries.GetMyAdoptionApplicationsQuery;
using PetShelter.Application.AdoptionApplications.Queries.GetMyPetsAdoptionApplicationsQuery;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdoptionApplicationController(ISender sender) : ApiController
    {
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyAdoptionApplications(
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = new GetMyAdoptionApplicationsQuery(
                status,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(success.ToPagedListResponse()),
                error => Problem(error)
            );
        }
        [HttpGet("my-pets-applications")]
        public async Task<IActionResult> GetMyPetsAdoptionApplications(
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var query = new GetMyPetsAdoptionApplicationQuery(
                status,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(success.ToPagedListResponse()),
                error => Problem(error)
            );
        }
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
