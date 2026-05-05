using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    public class AdoptionApplicationController(
        ISender sender,
        ILogger<AdoptionApplicationController> logger) : ApiController(logger)
    {
        [Authorize]
        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyAdoptionApplications(
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            logger.LogInformation("GET /api/adoptionapplication/my-applications called with status: {Status}, pageNumber: {PageNumber}, pageSize: {PageSize}", 
                status, pageNumber, pageSize);
            var query = new GetMyAdoptionApplicationsQuery(
                status,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/adoptionapplication/my-applications successful for userId: {UserId}", success.Items.FirstOrDefault()?.ApplicantId);
                    return Ok(success.ToPagedListResponse());
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpGet("my-pets-applications")]
        public async Task<IActionResult> GetMyPetsAdoptionApplications(
            [FromQuery] string? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            logger.LogInformation("GET /api/adoptionapplication/my-pets-applications called with status: {Status}, pageNumber: {PageNumber}, pageSize: {PageSize}", 
                status, pageNumber, pageSize);
            var query = new GetMyPetsAdoptionApplicationQuery(
                status,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/adoptionapplication/my-pets-applications successful for userId: {UserId}", success.Items.FirstOrDefault()?.ApplicantId);
                    return Ok(success.ToPagedListResponse());
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdoptionApplicationById(Guid id)
        {
            logger.LogInformation("GET /api/adoptionapplication/[id] called with id: {Id}", id);
            var query = new GetAdoptionApplicationByIdQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/adoptionapplication/[id] successful for id: {Id}", id);
                    return Ok(success.ToAdoptionApplicationResponse());
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectAdoptionApplication(Guid id)
        {
            logger.LogInformation("PUT /api/adoptionapplication/[id]/reject called with id: {Id}", id);
            var command = new RejectAdoptionApplicationCommand(id);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("PUT /api/adoptionapplication/[id]/reject successful for id: {Id}", id);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmAdoptionApplication(Guid id)
        {
            logger.LogInformation("PUT /api/adoptionapplication/[id]/confirm called with id: {Id}", id);
            var command = new ConfirmAdoptionApplicationCommand(id);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("PUT /api/adoptionapplication/[id]/confirm successful for id: {Id}", id);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdoptionApplication(CreateAdoptionApplicationRequest request)
        {
            logger.LogInformation("POST /api/adoptionapplication/create called with petId: {PetId}", request.PetId);
            var command = new CreateAdoptionApplicationCommand(
                Guid.Parse(request.PetId),
                request.Message
            );

            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("POST /api/adoptionapplication/create successful for petId: {PetId}", request.PetId);
                    return Ok(success.ToAdoptionApplicationResponse());
                },
                error => Problem(error)
            );
        }
    }
}
