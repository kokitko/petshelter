using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.UserProfile;
using PetShelter.Api.Mappings.Users;
using PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;

namespace PetShelter.Api.Controllers
{
    /// <summary>
    ///  This controller manages user profile operations, specifically allowing authenticated users to update their profile information such as phone number, profile picture, first name, and last name.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    public class UserProfileController(
        ISender sender,
        ILogger<UserProfileController> logger) : ApiController(logger)
    {
        [Authorize]
        [HttpPut("update")]
        [EndpointSummary("Update User Profile")]
        [EndpointDescription("Updates the profile information for a specific user. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileUpdateRequest request)
        {
            logger.LogInformation("PUT /api/userprofile/update called with phoneNumber: {PhoneNumber}, firstName: {FirstName}, lastName: {LastName}", request.PhoneNumber, request.FirstName, request.LastName);
            var command = new UserProfileUpdateCommand(
                request.PhoneNumber,
                request.ProfilePicture,
                request.FirstName,
                request.LastName
            );

            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("PUT /api/userprofile/update successful for userId: {UserId}", success.Id);
                    return Ok(success.ToUserProfileResponse());
                },
                error => Problem(error)
            );
        }
    }
}
