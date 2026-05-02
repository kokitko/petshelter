using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Accounts.Commands.ChangeEmailCommand;

public class ChangeEmailCommandHandler(
    IAppUserRepository userRepository,
    ICurrentUserProvider currentUserProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenRepository
) : IRequestHandler<ChangeEmailCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
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

        if (!passwordHasher.VerifyPassword(user.PasswordHash, request.CurrentPassword))
            return Errors.Authentication.InvalidCredentials;

        user.Email = request.NewEmail;
        await userRepository.UpdateAsync(user);
        await refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        return true;
    }
}
