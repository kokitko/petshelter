using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.Account;
using MediatR;
using PetShelter.Application.Accounts.Commands.ChangePasswordCommand;
using PetShelter.Application.Accounts.Commands.ChangeEmailCommand;
using PetShelter.Application.Accounts.Commands.DeleteAccountCommand;
using PetShelter.Application.Accounts.Queries.GetAccountInfoQuery;
using PetShelter.Domain.Entities;
using PetShelter.Api.Mappings.Organizations;
using PetShelter.Api.Mappings.Users;
using PetShelter.Application.Accounts.Queries.GetMyAccountInfoQuery;
using Microsoft.AspNetCore.Authorization;

namespace PetShelter.Api.Controllers
{
    /// <summary>
    ///  This controller handles all account-related operations, including retrieving account information, changing email and password, and deleting accounts.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="logger"></param>
    [Route("api/[controller]")]
    public class AccountController(
        ISender sender,
        ILogger<AccountController> logger) : ApiController(logger)
    {
        [Authorize]
        [HttpGet("me")]
        [EndpointSummary("Get My Account Info")]
        [EndpointDescription("Retrieves the account information of the currently authenticated user. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMyAccountInfo()
        {
            logger.LogInformation("GET /api/account/me called");
            var query = new GetMyAccountInfoQuery();
            var result = await sender.Send(query);
            return result.Match(
                success =>
                {
                    logger.LogInformation("GET /api/account/me successful for userId: {UserId}", success.Id);
                    if (success.Role == UserRole.Organization.ToString())
                        return Ok(success.ToOrgProfileResponse());
                    else
                        return Ok(success.ToUserProfileResponse());
                },
                error => Problem(error)
            );
        }
        [HttpGet("{id}")]
        [EndpointSummary("Get Account Info by ID")]
        [EndpointDescription("Retrieves the account information for a specific user by their ID. This endpoint is public and does not require authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountInfo(Guid id)
        {
            logger.LogInformation("GET /api/account/[id] called with id: {Id}", id);
            var query = new GetAccountInfoQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success =>
                {
                    logger.LogInformation("GET /api/account/[id] successful for id: {Id}", id);
                    if (success.Role == UserRole.Organization.ToString())
                        return Ok(success.ToOrgProfileResponse());
                    else
                        return Ok(success.ToUserProfileResponse());
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("change-password")]
        [EndpointSummary("Change Password")]
        [EndpointDescription("Changes the password for the currently authenticated user. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            logger.LogInformation("PUT /api/account/change-password called");
            var command = new ChangePasswordCommand(
                request.CurrentPassword,
                request.NewPassword
            );

            var result = await sender.Send(command);
            return result.Match(
                success =>
                {
                    logger.LogInformation("Password change successful for userId: {UserId}", success.Id);
                    ClearRefreshTokenCookie();
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpPut("change-email")]
        [EndpointSummary("Change Email")]
        [EndpointDescription("Changes the email for the currently authenticated user. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
        {
            logger.LogInformation("PUT /api/account/change-email called");
            var command = new ChangeEmailCommand(
                request.NewEmail,
                request.CurrentPassword
            );

            var result = await sender.Send(command);
            return result.Match(
                success =>
                {
                    logger.LogInformation("Email change successful for userId: {UserId}", success.Id);
                    ClearRefreshTokenCookie();
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [Authorize]
        [HttpDelete("delete")]
        [EndpointSummary("Delete Account")]
        [EndpointDescription("Deletes the account for the currently authenticated user. Requires authentication.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAccount(DeleteAccountRequest request)
        {
            logger.LogInformation("DELETE /api/account/delete called");
            var command = new DeleteAccountCommand(request.Password);

            var result = await sender.Send(command);
            return result.Match(
                success =>
                {
                    logger.LogInformation("Account deletion successful for userId: {UserId}", success.Id);
                    ClearRefreshTokenCookie();
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        private void ClearRefreshTokenCookie()
        {
            Response.Cookies.Delete("refreshToken");
        }
    }
}