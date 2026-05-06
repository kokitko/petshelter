using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Api.Mappings.Organizations;
using PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrgProfileController(
        ISender sender, 
        ILogger<OrgProfileController> logger) : ApiController(logger)
    {
        [HttpGet]
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
