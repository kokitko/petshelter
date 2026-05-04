using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;

public class UserProfileUpdateCommandHandler(
    IAppUserRepository userRepository,
    ICurrentUserProvider currentUserProvider,
    IFileStorageService fileStorageService
) : IRequestHandler<UserProfileUpdateCommand, ErrorOr<ReturnAppUserDto>>
{
    public async Task<ErrorOr<ReturnAppUserDto>> Handle(UserProfileUpdateCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var user = await userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            return Errors.UserProfile.NotFound;

        if (user.Role != Domain.Entities.UserRole.User)
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
        
        user.UserProfile!.FirstName = request.FirstName;
        user.UserProfile!.LastName = request.LastName;

        await userRepository.UpdateAsync(user);

        return user.ToReturnAppUserDto();
    }
}
