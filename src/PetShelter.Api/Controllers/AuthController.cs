using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetShelter.Api.Contracts.Authentication;
using PetShelter.Application.Authentication.Commands.Login;
using PetShelter.Application.Authentication.Commands.RefreshToken;
using PetShelter.Api.Mappings.Authentication;
using PetShelter.Application.Authentication.Commands.Logout;

namespace PetShelter.Api.Controllers
{
    /// <summary>
    ///  This controller handles all authentication-related operations, including user registration, login, logout, and token refreshing. It provides endpoints for users to authenticate themselves and manage their authentication tokens.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    public class AuthController(
        ISender sender,
        ILogger<AuthController> logger) : ApiController(logger)
    {
        [HttpPost("logout")]
        [EndpointSummary("Logout")]
        [EndpointDescription("Logs out the current user and invalidates their refresh token.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            logger.LogInformation("POST /api/auth/logout called");
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return BadRequest(new { Message = "Refresh token is missing." });
            
            var command = new LogoutCommand(refreshToken);
            await sender.Send(command);

            Response.Cookies.Delete("refreshToken");
            logger.LogInformation("POST /api/auth/logout successful, refresh token deleted for token: {RefreshToken}", refreshToken);
            return Ok();
        }

        [HttpPost("register")]
        [EndpointSummary("Register")]
        [EndpointDescription("Registers a new user account.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            logger.LogInformation("POST /api/auth/register called with email: {Email}", request.Email);
            var command = request.ToRegisterUserCommand();

            var result = await sender.Send(command);

            return result.Match(
                authResult =>
                {
                    logger.LogInformation("POST /api/auth/register successful for email: {Email}, userId: {UserId}", request.Email, authResult.User.Id);
                    SetRefreshTokenCookie(authResult.RefreshToken, Response);
                    var response = authResult.ToAuthResponse();
                    return Ok(response);
                },
                errors => Problem(errors));
        }

        [HttpPost("login")]
        [EndpointSummary("Login")]
        [EndpointDescription("Logs in a user and returns an authentication token.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            logger.LogInformation("POST /api/auth/login called with email: {Email}", request.Email);
            var command = new LoginCommand(request.Email, request.Password);
            var result = await sender.Send(command);

            return result.Match(
                authResult =>
                {
                    logger.LogInformation("POST /api/auth/login successful for email: {Email}, userId: {UserId}", request.Email, authResult.User.Id);
                    SetRefreshTokenCookie(authResult.RefreshToken, Response);
                    var response = authResult.ToAuthResponse();
                    return Ok(response);
                },
                errors => Problem(errors));
        }

        [HttpPost("refresh-token")]
        [EndpointSummary("Refresh Token")]
        [EndpointDescription("Generates a new authentication token using a valid refresh token.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken()
        {
            logger.LogInformation("POST /api/auth/refresh-token called");
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                logger.LogWarning("POST /api/auth/refresh-token called without refresh token");
                return BadRequest(new { Message = "Refresh token is missing." });
            }

            var command = new RefreshTokenCommand(refreshToken);
            var result = await sender.Send(command);

            return result.Match(
                authResult =>
                {
                    logger.LogInformation("POST /api/auth/refresh-token successful for userId: {UserId}", authResult.User.Id);
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
