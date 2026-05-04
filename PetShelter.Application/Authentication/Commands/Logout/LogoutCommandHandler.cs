using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Authentication.Commands.Logout;

public class LogoutCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    ICurrentUserProvider currentUserProvider
) : IRequestHandler<LogoutCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        var userId = currentUserProvider.GetCurrentUserId();

        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        if (refreshToken is null)
            return Errors.Authentication.InvalidRefreshToken;

        if (refreshToken?.UserId != userId || refreshToken.IsActive != true)
            return Errors.Authentication.InvalidRefreshToken;

        await refreshTokenRepository.DeactivateAsync(refreshToken.Id);

        return true;
    }
}
