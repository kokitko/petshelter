using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Admin.Commands.AdminUpdateAccountCommand;

public record AdminUpdateAccountCommand(
    // basic user info
    string UserId,
    string? Email,
    string? Password,
    string? PhoneNumber,
    IFormFile? ProfilePicture,
    // user profile info
    string? FirstName,
    string? LastName,
    // org profile info
    string? OrgName,
    string? Address,
    string? Website,
    bool? IsVerified
) : IRequest<ErrorOr<ReturnAppUserDto>>;