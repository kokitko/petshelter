using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShelter.Api.Mappings.Users;
using PetShelter.Application.Admin.Queries.AdminGetUsersQuery;
using PetShelter.Application.Admin.Queries.AdminGetOrganizationsQuery;
using PetShelter.Api.Mappings.Organizations;
using PetShelter.Application.Admin.Commands.AdminUpdateAccountCommand;
using PetShelter.Api.Contracts.Admin;
using PetShelter.Domain.Entities;
using PetShelter.Application.Admin.Commands.AdminDeleteAccountCommand;
using PetShelter.Application.Admin.Commands.AdminUpdatePetCommand;
using PetShelter.Api.Mappings.Pets;
using PetShelter.Application.Admin.Commands.AdminDeletePetCommand;
using PetShelter.Application.Admin.Commands.AdminDeleteAdoptionApplicationCommand;

namespace PetShelter.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController(
        ISender sender,
        ILogger<AdminController> logger
    ) : ApiController(logger)
    {
        [HttpDelete("applications/{applicationId}")]
        public async Task<IActionResult> DeleteAdoptionApplication([FromRoute] string applicationId)
        {
            logger.LogInformation("DELETE /api/admin/applications/{ApplicationId} called", applicationId);
            var command = new AdminDeleteAdoptionApplicationCommand(applicationId);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("DELETE /api/admin/applications/{ApplicationId} successful", applicationId);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [HttpDelete("pets/{petId}")]
        public async Task<IActionResult> DeletePet([FromRoute] string petId)
        {
            logger.LogInformation("DELETE /api/admin/pets/{PetId} called", petId);
            var command = new AdminDeletePetCommand(petId);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("DELETE /api/admin/pets/{PetId} successful", petId);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }

        [HttpPut("pets/{petId}")]
        public async Task<IActionResult> UpdatePet(
            [FromRoute] string petId,
            [FromForm] AdminUpdatePetRequest request)
        {
            logger.LogInformation("PUT /api/admin/pets/{PetId} called with name: {Name}, species: {Species}, age: {Age}, description: {Description}, mainPicture: {MainPicture}, picturesToAddCount: {PicturesToAddCount}, pictureIdsToRemoveCount: {PictureIdsToRemoveCount}", 
                petId, request.Name, request.Species, request.Age, request.Description, request.MainPicture != null ? request.MainPicture.FileName : "null", request.PicturesToAdd != null ? request.PicturesToAdd.Count : 0, request.PictureIdsToRemove != null ? request.PictureIdsToRemove.Count : 0);
            var command = new AdminUpdatePetCommand(
                petId,
                request.Name,
                request.Species,
                request.Breed,
                request.Age,
                request.Description,
                request.MainPicture,
                request.PicturesToAdd,
                request.PictureIdsToRemove
            );

            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("PUT /api/admin/pets/[PetId] successful for pet with ID: {PetId}", petId);
                    return Ok(success.ToPetResponse());
                },
                error => Problem(error)
            );
        }
        [HttpDelete("accounts/{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            logger.LogInformation("DELETE /api/admin/accounts/{UserId} called", userId);
            var command = new AdminDeleteAccountCommand(userId);
            var result = await sender.Send(command);
            return result.Match(
                success => {
                    logger.LogInformation("DELETE /api/admin/accounts/{UserId} successful", userId);
                    return Ok(success);
                },
                error => Problem(error)
            );
        }
        [HttpPut("accounts/{userId}")]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] string userId, 
            [FromForm] AdminUpdateAccountRequest request)
        {
            logger.LogInformation("PUT /api/admin/accounts/{UserId} called with email: {Email}, phoneNumber: {PhoneNumber}, firstName: {FirstName}, lastName: {LastName}, orgName: {OrgName}, address: {Address}, website: {Website}, isVerified: {IsVerified}", 
                userId, request.Email, request.PhoneNumber, request.FirstName, request.LastName, request.OrgName, request.Address, request.Website, request.IsVerified);
            var command = new AdminUpdateAccountCommand(
                userId,
                request.Email,
                request.Password,
                request.PhoneNumber,
                request.ProfilePicture,
                request.FirstName,
                request.LastName,
                request.OrgName,
                request.Address,
                request.Website,
                request.IsVerified
            );

            var result = await sender.Send(command);
            return result.Match(
                success => {
                    if (success.Role == UserRole.Organization.ToString())
                    {
                        logger.LogInformation("PUT /api/admin/accounts/[UserId] successful for organization with ID: {UserId}", userId);
                        return Ok(success.ToOrgProfileResponse());
                    }
                    else
                    {
                        logger.LogInformation("PUT /api/admin/accounts/[UserId] successful for user with ID: {UserId}", userId);
                        return Ok(success.ToUserProfileResponse());
                    }
                },
                error => Problem(error)
            );

        }
        [HttpGet("orgs")]
        public async Task<IActionResult> GetOrgProfiles(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? orgName = null,
            [FromQuery] string? address = null,
            [FromQuery] bool? isVerified = null
        )
        {
            logger.LogInformation("GET /api/admin/orgs called with pageNumber: {PageNumber}, pageSize: {PageSize}, orgName: {OrgName}, address: {Address}, isVerified: {IsVerified}", 
                pageNumber, pageSize, orgName, address, isVerified);
            var query = new AdminGetOrganizationsQuery(
                orgName,
                address,
                isVerified,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/admin/orgs successful with totalCount: {TotalCount}", success.TotalCount);
                    return Ok(success.ToPagedOrgListResponse());
                },
                error => Problem(error)
            );
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? firstName = null,
            [FromQuery] string? lastName = null
        )
        {
            logger.LogInformation("GET /api/admin/users called with pageNumber: {PageNumber}, pageSize: {PageSize}, firstName: {FirstName}, lastName: {LastName}", 
                pageNumber, pageSize, firstName, lastName);
            var query = new AdminGetUsersQuery(
                firstName,
                lastName,
                pageNumber,
                pageSize
            );
            var result = await sender.Send(query);
            return result.Match(
                success => {
                    logger.LogInformation("GET /api/admin/users successful with totalCount: {TotalCount}", success.TotalCount);
                    return Ok(success.ToPagedUserListResponse());
                },
                error => Problem(error)
            );
        }
    }
}
