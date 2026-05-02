using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;
using PetShelter.Application.OrgProfiles.Queries.GetOrgProfileQuery;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrgProfileController(ISender sender) : ApiController
    {
        [HttpGet("/{id}/pets")]
        public async Task<IActionResult> GetOrgPets(Guid id)
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrgProfile(Guid id)
        {
            var query = new GetOrgProfileQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(
                    new OrgProfileResponse(
                        success.Id.ToString(),
                        success.OrgProfile!.OrgName,
                        success.OrgProfile!.Address,
                        success.OrgProfile!.Website,
                        success.Email,
                        success.PhoneNumber,
                        success.ProfilePictureUrl,
                        success.Role
                    )
                ),
                error => Problem(error)
            );
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateOrgProfile([FromForm] OrgProfileUpdateRequest request)
        {
            var command = new OrgProfileUpdateCommand(
                request.PhoneNumber,
                request.ProfilePicture,
                request.OrgName,
                request.Address,
                request.Website
            );

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(
                    new OrgProfileResponse(
                        success.Id.ToString(),
                        success.OrgProfile!.OrgName,
                        success.OrgProfile!.Address,
                        success.OrgProfile!.Website,
                        success.Email,
                        success.PhoneNumber,
                        success.ProfilePictureUrl,
                        success.Role
                    )
                ),
                error => Problem(error)
            );
        }
    }
}
