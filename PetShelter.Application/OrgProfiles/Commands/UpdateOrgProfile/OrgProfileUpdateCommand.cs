using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;

public record OrgProfileUpdateCommand(
    string Email,
    string? PhoneNumber,
    IFormFile? ProfilePicture,
    string OrgName,
    string Address,
    string? Website
) : IRequest<ErrorOr<ReturnAppUserDto>>;