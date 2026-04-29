using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Application.Authentication.Commands;
using PetShelter.Api.Contracts.Authentication;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(ISender sender) : ApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.Password,
                request.PhoneNumber,

                request.OrgProfile is not null
                    ? new OrgProfileInfo(
                        request.OrgProfile.OrgName,
                        request.OrgProfile.Address,
                        request.OrgProfile.Website)
                    : null,

                request.UserProfile is not null
                    ? new UserProfileInfo(
                        request.UserProfile.FirstName,
                        request.UserProfile.LastName)
                    : null

            );
            var result = await sender.Send(command);

            // TO REWRITE: should return accesstoken and user, refreshtoken in the cookie
            return result.Match(
                authResult => Ok(authResult),
                errors => Problem(errors));
        }
    }
}
