using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Admin.Commands.AdminDeleteAccountCommand;

public class AdminDeleteAccountCommandHandler(
    IAppUserRepository userRepository,
    IPetRepository petRepository,
    IPetImageRepository petImageRepository,
    IAdoptionApplicationRepository applicationRepository,
    IOrgProfileRepository orgProfileRepository,
    IUserProfileRepository userProfileRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IFileStorageService fileService,
    ICacheService cacheService
) : IRequestHandler<AdminDeleteAccountCommand, ErrorOr<ChangeSensitiveInfoDto>>
{
    public async Task<ErrorOr<ChangeSensitiveInfoDto>> Handle(AdminDeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId);
        var user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return Errors.UserProfile.NotFound;

        if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            await fileService.DeleteAsync(user.ProfilePictureUrl);

        if (user.OrgProfile != null)
        {
            await orgProfileRepository.DeleteAsync(user.OrgProfile);
            await cacheService.RemoveByPrefixAsync("GetOrganizationsQuery", cancellationToken);
        }
        else if (user.UserProfile != null)
        {
            await userProfileRepository.DeleteAsync(user.UserProfile);
        }

        foreach (var pet in user.Pets)
        {
            foreach (var image in pet.Images)
            {
                await fileService.DeleteAsync(image.Url);
                await petImageRepository.DeleteAsync(image);
            }
            foreach (var adoption in pet.Applications)
                if (adoption.PetId == pet.Id)
                    await applicationRepository.DeleteAsync(adoption);

            await petRepository.DeleteAsync(pet);
        }

        var applicantApplications = user.Applications?.ToList() ?? new List<AdoptionApplication>();
        foreach (var application in applicantApplications)
            await applicationRepository.DeleteAsync(application);

        foreach (var token in user.RefreshTokens)
            await refreshTokenRepository.DeleteAsync(token);

        await userRepository.DeleteAsync(user);

        return new ChangeSensitiveInfoDto(request.UserId);
    }
}
