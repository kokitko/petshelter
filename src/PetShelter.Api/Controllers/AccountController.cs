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
    [Route("api/[controller]")]
    public class AccountController(
        ISender sender,
        ILogger<AccountController> logger) : ApiController(logger)
    {
        [Authorize]
        [HttpGet("me")]
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