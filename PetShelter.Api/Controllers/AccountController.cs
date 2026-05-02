using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Contracts.Account;
using MediatR;
using PetShelter.Application.Accounts.Commands.ChangePasswordCommand;
using PetShelter.Application.Accounts.Commands.ChangeEmailCommand;

namespace PetShelter.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController(ISender sender) : ApiController
    {
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

        private void ClearRefreshTokenCookie()
        {
            Response.Cookies.Delete("refreshToken");
        }
    }
}