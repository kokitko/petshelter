using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;

public record UserProfileUpdateCommand(
    string? PhoneNumber,
    IFormFile? ProfilePicture,
    string FirstName,
    string LastName
) : IRequest<ErrorOr<ReturnAppUserDto>>;
