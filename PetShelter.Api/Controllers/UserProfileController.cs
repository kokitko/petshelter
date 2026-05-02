using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;
using PetShelter.Application.UserProfiles.Queries.GetUserProfileQuery;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController(ISender sender) : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(Guid id)
        {
            var query = new GetUserProfileQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success => Ok(
                    new UserProfileResponse(
                        success.Id.ToString(),
                        success.UserProfile!.FirstName,
                        success.UserProfile!.LastName,
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
                success => Ok(
                    new UserProfileResponse(
                        success.Id.ToString(),
                        success.UserProfile!.FirstName,
                        success.UserProfile!.LastName,
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
