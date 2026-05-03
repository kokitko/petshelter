using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Api.Mappings.Users;
using PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController(ISender sender) : ApiController
    {
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileUpdateRequest request)
        {
            var command = new UserProfileUpdateCommand(
                request.PhoneNumber,
                request.ProfilePicture,
                request.FirstName,
                request.LastName
            );

            var result = await sender.Send(command);
            return result.Match(
                success => Ok(success.ToUserProfileResponse()),
                error => Problem(error)
            );
        }
    }
}
