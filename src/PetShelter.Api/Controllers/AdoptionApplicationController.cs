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
    /// <summary>
    ///  This controller manages all operations related to adoption applications, including creating new applications, retrieving application details, and updating application status (confirming or rejecting). All endpoints require authentication, and some may have additional constraints (e.g., only pet owners can confirm applications for their pets).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    public class AdoptionApplicationController(
        ISender sender,
        ILogger<AdoptionApplicationController> logger) : ApiController(logger)
    {
        [Authorize]
        [HttpGet("my-applications")]
        [EndpointSummary("Get My Adoption Applications")]
        [EndpointDescription("Retrieves a list of adoption applications submitted by the currently authenticated user.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [EndpointSummary("Get My Pets' Adoption Applications")]
        [EndpointDescription("Retrieves a list of adoption applications for pets owned by the currently authenticated user.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [EndpointSummary("Get Adoption Application by ID")]
        [EndpointDescription("Retrieves the details of a specific adoption application by its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Reject Adoption Application")]
        [EndpointDescription("Rejects a specific adoption application by its ID. Requires authentication. Can be called by pet owners and applicants.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Confirm Adoption Application")]
        [EndpointDescription("Confirms a specific adoption application by its ID. Requires authentication. Can be called by pet owners.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [EndpointSummary("Create Adoption Application")]
        [EndpointDescription("Creates a new adoption application for a specific pet. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
