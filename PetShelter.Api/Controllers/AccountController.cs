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

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController(ISender sender) : ApiController
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccountInfo()
        {
            var query = new GetMyAccountInfoQuery();
            var result = await sender.Send(query);
            return result.Match(
                success =>
                {
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
            var query = new GetAccountInfoQuery(id);
            var result = await sender.Send(query);
            return result.Match(
                success =>
                {
                    if (success.Role == UserRole.Organization.ToString())
                        return Ok(success.ToOrgProfileResponse());
                    else
                        return Ok(success.ToUserProfileResponse());
                },
                error => Problem(error)
            );
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var command = new ChangePasswordCommand(
                request.CurrentPassword,
                request.NewPassword
            );

            var result = await sender.Send(command);
            return result.Match(
                success =>
                {
                    ClearRefreshTokenCookie();
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [HttpPut("change-email")]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
        {
            var command = new ChangeEmailCommand(
                request.NewEmail,
                request.CurrentPassword
            );

            var result = await sender.Send(command);
            return result.Match(
                success =>
                {
                    ClearRefreshTokenCookie();
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount(DeleteAccountRequest request)
        {
            var command = new DeleteAccountCommand(request.Password);

            var result = await sender.Send(command);
            return result.Match(
                success =>
                {
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