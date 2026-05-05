using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Api.Mappings.Organizations;
using PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrgProfileController(ISender sender) : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetOrgProfiles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orgName = null,
            [FromQuery] string? address = null
        )
        {
            var query = new GetOrganizationsQuery(
                orgName,
                address,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);

            return result.Match(
                success => Ok(success.ToPagedListResponse()),
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateOrgProfile([FromForm] OrgProfileUpdateRequest request)
        {
            var command = request.ToOrgProfileUpdateCommand();

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(success.ToOrgProfileResponse()),
                error => Problem(error)
            );
        }
    }
}
