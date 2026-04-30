using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Application.Authentication.Commands;
using PetShelter.Api.Contracts.Authentication;
using PetShelter.Application.Authentication.Commands.Login;
using PetShelter.Application.Authentication.Commands.RefreshToken;

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

            return result.Match(
                authResult =>
                {
                    SetRefreshTokenCookie(authResult.RefreshToken, Response);
                    var response = new AuthResponse(
                        authResult.AccessToken,
                        new UserAuthResponse(
                            authResult.User.Id,
                            authResult.User.Email,
                            authResult.User.PhoneNumber,
                            authResult.User.Role
                        )
                    );
                    return Ok(response);
                },
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await sender.Send(command);

            return result.Match(
                authResult =>
                {
                    SetRefreshTokenCookie(authResult.RefreshToken, Response);
                    var response = new AuthResponse(
                        authResult.AccessToken,
                        new UserAuthResponse(
                            authResult.User.Id,
                            authResult.User.Email,
                            authResult.User.PhoneNumber,
                            authResult.User.Role
                        )
                    );
                    return Ok(response);
                },
                errors => Problem(errors));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return BadRequest(new { Message = "Refresh token is missing." });
            }

            var command = new RefreshTokenCommand(refreshToken);
            var result = await sender.Send(command);

            return result.Match(
                authResult =>
                {
                    SetRefreshTokenCookie(authResult.RefreshToken, Response);
                    var response = new AuthResponse(
                        authResult.AccessToken,
                        new UserAuthResponse(
                            authResult.User.Id,
                            authResult.User.Email,
                            authResult.User.PhoneNumber,
                            authResult.User.Role
                        )
                    );
                    return Ok(response);
                },
                errors => Problem(errors));
        }

        private static void SetRefreshTokenCookie(string refreshToken, HttpResponse response)
        {
            response.Cookies.Delete("refreshToken");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(14),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            
            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
