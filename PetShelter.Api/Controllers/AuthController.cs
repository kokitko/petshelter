using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Api.Contracts.Authentication;
using PetShelter.Application.Authentication.Commands.Login;
using PetShelter.Application.Authentication.Commands.RefreshToken;
using PetShelter.Api.Mappings.Authentication;
using PetShelter.Application.Authentication.Commands.Logout;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(ISender sender) : ApiController
    {
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return BadRequest(new { Message = "Refresh token is missing." });
            
            var command = new LogoutCommand(refreshToken);
            await sender.Send(command);

            Response.Cookies.Delete("refreshToken");
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var command = request.ToRegisterUserCommand();

            var result = await sender.Send(command);

            return result.Match(
                authResult =>
                {
                    SetRefreshTokenCookie(authResult.RefreshToken, Response);
                    var response = authResult.ToAuthResponse();
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
                    var response = authResult.ToAuthResponse();
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
                    var response = authResult.ToAuthResponse();
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
