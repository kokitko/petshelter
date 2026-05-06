using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Admin.Commands.AdminUpdateAccountCommand;

public class AdminUpdateAccountCommandHandler(
    IAppUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IFileStorageService fileStorageService,
    ICacheService cacheService
) : IRequestHandler<AdminUpdateAccountCommand, ErrorOr<ReturnAppUserDto>>
{
    public async Task<ErrorOr<ReturnAppUserDto>> Handle(AdminUpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(request.UserId));
        if (user == null)
            return Errors.Accounts.NotFound;

        if (request.Email != null)
            user.Email = request.Email;
        if (request.Password != null)
            user.PasswordHash = passwordHasher.HashPassword(request.Password);
        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;
        if (request.ProfilePicture != null)
        {
            string newProfilePictureUrl = await fileStorageService.UploadFileAsync(
                request.ProfilePicture, 
                request.ProfilePicture.FileName, 
                request.ProfilePicture.ContentType);

            if (!string.IsNullOrEmpty(newProfilePictureUrl) && !string.IsNullOrEmpty(user.ProfilePictureUrl))
                await fileStorageService.DeleteAsync(user.ProfilePictureUrl);

            user.ProfilePictureUrl = newProfilePictureUrl;
        }

        if (user.Role == UserRole.Organization)
        {
            if (request.OrgName != null)
                user.OrgProfile!.OrgName = request.OrgName;
            if (request.Address != null)
                user.OrgProfile!.Address = request.Address;
            if (request.Website != null)
                user.OrgProfile!.Website = request.Website;
            if (request.IsVerified != null)
                user.OrgProfile!.IsVerified = request.IsVerified.Value;
        }
        else
        {
            if (request.FirstName != null)
                user.UserProfile!.FirstName = request.FirstName;
            if (request.LastName != null)
                user.UserProfile!.LastName = request.LastName;
        }

        await userRepository.UpdateAsync(user);
        await cacheService.RemoveByPrefixAsync("GetOrganizationsQuery", cancellationToken);
        return user.ToReturnAppUserDto();
    }
}
