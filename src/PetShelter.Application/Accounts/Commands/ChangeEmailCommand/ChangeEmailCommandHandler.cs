using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Accounts.Commands.ChangeEmailCommand;

public class ChangeEmailCommandHandler(
    IAppUserRepository userRepository,
    ICurrentUserProvider currentUserProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenRepository,
    ICacheService cacheService
) : IRequestHandler<ChangeEmailCommand, ErrorOr<ChangeSensitiveInfoDto>>
{
    public async Task<ErrorOr<ChangeSensitiveInfoDto>> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var user = await userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            return Errors.Accounts.NotFound;

        var existingUserWithEmail = await userRepository.GetByEmailAsync(request.NewEmail);
        if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
            return Errors.Authentication.DuplicateEmail;

        if (!passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            return Errors.Authentication.InvalidCredentials;

        user.Email = request.NewEmail;
        await userRepository.UpdateAsync(user);
        await refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        await cacheService.RemoveByPrefixAsync("GetOrganizationsQuery", cancellationToken);
        
        return new ChangeSensitiveInfoDto(user.Id.ToString());
    }
}
