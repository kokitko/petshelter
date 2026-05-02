using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.OrgProfile;
using PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrgProfileController(ISender sender) : ApiController
    {
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
                    new OrgProfileUpdateResponse
                    (
                        success.Id.ToString(),
                        success.Email,
                        success.PhoneNumber,
                        success.ProfilePictureUrl,
                        success.Role,
                        new OrgProfileUpdateInfo(
                            success.OrgProfile!.OrgName,
                            success.OrgProfile!.Address,
                            success.OrgProfile!.Website
                        )
                    )
                ),
                error => Problem(error)
            );
        }
    }
}
