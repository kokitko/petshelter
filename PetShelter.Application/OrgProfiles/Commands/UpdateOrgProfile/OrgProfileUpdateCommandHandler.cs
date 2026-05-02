using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;

public class OrgProfileUpdateCommandHandler(
    IAppUserRepository userRepository,
    ICurrentUserProvider currentUserProvider,
    IFileStorageService fileStorageService
) : IRequestHandler<OrgProfileUpdateCommand, ErrorOr<ReturnAppUserDto>>
{
    public async Task<ErrorOr<ReturnAppUserDto>> Handle(OrgProfileUpdateCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var user = await userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            return Errors.OrgProfile.NotFound;

        if (user.Role != Domain.Entities.UserRole.Organization)
            return Errors.Authentication.Forbidden;

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
        
        user.OrgProfile!.OrgName = request.OrgName;
        user.OrgProfile!.Address = request.Address;
        user.OrgProfile!.Website = request.Website;

        await userRepository.UpdateAsync(user);

        return new ReturnAppUserDto(
            user.Id,
            user.Email,
            user.PhoneNumber,
            user.ProfilePictureUrl,
            user.Role.ToString(),
            null,
            new ReturnOrgProfileInfo(
                user.OrgProfile.OrgName,
                user.OrgProfile.Address,
                user.OrgProfile.Website,
                user.OrgProfile.IsVerified
            )
        );
    }
}
