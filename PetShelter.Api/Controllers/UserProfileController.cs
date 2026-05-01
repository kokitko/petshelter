using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController(ISender sender) : ApiController
    {
        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileUpdateRequest request)
        {
            var command = new UserProfileUpdateCommand(
                request.Email,
                request.PhoneNumber,
                request.ProfilePicture,
                request.FirstName,
                request.LastName
            );

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(
                    new UserProfileUpdateResponse
                    (
                        success.Id.ToString(),
                        success.Email,
                        success.PhoneNumber,
                        success.ProfilePictureUrl,
                        success.Role,
                        new UserProfileUpdateInfo(
                            success.UserProfile!.FirstName,
                            success.UserProfile!.LastName
                        )
                    )
                ),
                error => Problem(error)
            );
        }
    }
}
