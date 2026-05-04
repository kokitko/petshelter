using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Accounts.Commands.DeleteAccountCommand;

public class DeleteAccountCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IAppUserRepository userRepository,
    IOrgProfileRepository orgProfileRepository,
    IUserProfileRepository userProfileRepository,
    IPetRepository petRepository,
    IPetImageRepository petImageRepository,
    IFileStorageService fileService,
    IAdoptionApplicationRepository adoptionRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPasswordHasher passwordHasher
) : IRequestHandler<DeleteAccountCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;
            
        var user = await userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            return Errors.UserProfile.NotFound;

        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Errors.Authentication.InvalidCredentials;

        if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            await fileService.DeleteAsync(user.ProfilePictureUrl);

        if (user.OrgProfile != null)
            await orgProfileRepository.DeleteAsync(user.OrgProfile);
        else if (user.UserProfile != null)
            await userProfileRepository.DeleteAsync(user.UserProfile);

        foreach (var pet in user.Pets)
        {
            foreach (var image in pet.Images)
            {
                await fileService.DeleteAsync(image.Url);
                await petImageRepository.DeleteAsync(image);
            }
            foreach (var adoption in pet.Applications)
                if (adoption.PetId == pet.Id)
                    await adoptionRepository.DeleteAsync(adoption);

            await petRepository.DeleteAsync(pet);
        }

        foreach (var application in user.Applications)
            await adoptionRepository.DeleteAsync(application);

        foreach (var token in user.RefreshTokens)
            await refreshTokenRepository.DeleteAsync(token);

        await userRepository.DeleteAsync(user);
        return true;
    }
}
