using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Accounts.Commands.ChangePasswordCommand;

public class ChangePasswordCommandHandler(
    IAppUserRepository userRepository,
    ICurrentUserProvider currentUserProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenRepository
) : IRequestHandler<ChangePasswordCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var user = await userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            return Errors.Accounts.NotFound;

        if (!passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            return Errors.Authentication.InvalidCredentials;

        user.PasswordHash = passwordHasher.HashPassword(request.NewPassword);
        await userRepository.UpdateAsync(user);
        await refreshTokenRepository.DeleteByUserIdAsync(userId.Value);
        return true;
    }
}
