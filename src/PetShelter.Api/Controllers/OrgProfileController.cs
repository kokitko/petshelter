using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Api.Mappings.Organizations;
using PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

namespace PetShelter.Api.Controllers
{
    /// <summary>
    ///  This controller handles all operations related to organization profiles, including retrieving a list of organizations and updating an organization's profile information. It provides endpoints for clients to interact with organization data, allowing them to view and manage their profiles as needed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    public class OrgProfileController(
        ISender sender, 
        ILogger<OrgProfileController> logger) : ApiController(logger)
    {
        [HttpGet]
        [EndpointSummary("Get Organization Profiles")]
        [EndpointDescription("Retrieves a list of organization profiles based on the provided filters.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrgProfiles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orgName = null,
            [FromQuery] string? address = null,
            [FromQuery] bool? isVerified = null
        )
        {
            logger.LogInformation("GET /api/orgprofile called with pageNumber: {PageNumber}, pageSize: {PageSize}, orgName: {OrgName}, address: {Address}, isVerified: {IsVerified}", 
                pageNumber, pageSize, orgName, address, isVerified);
            var query = new GetOrganizationsQuery(
                orgName,
                address,
                isVerified,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);

            return result.Match(
                success => {
                    logger.LogInformation("GET /api/orgprofile successful with {Count} results", success.TotalCount);
                    return Ok(success.ToPagedOrgListResponse());
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("update")]
        [EndpointSummary("Update Organization Profile")]
        [EndpointDescription("Updates the profile information for a specific organization. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrgProfile([FromForm] OrgProfileUpdateRequest request)
        {
            logger.LogInformation("PUT /api/orgprofile/update called with orgName: {OrgName}, address: {Address}", request.OrgName, request.Address);
            var command = request.ToOrgProfileUpdateCommand();

            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("PUT /api/orgprofile/update successful for orgId: {OrgId}", success.Id);
                    return Ok(success.ToOrgProfileResponse());
                },
                error => Problem(error)
            );
        }
    }
}
