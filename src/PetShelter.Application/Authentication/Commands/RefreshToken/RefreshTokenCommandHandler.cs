using ErrorOr;
using MediatR;
using PetShelter.Application.Authentication.Common;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IAppUserRepository userRepository
    ) : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken is null || !refreshToken.IsActive)
            return Errors.Authentication.InvalidRefreshToken;

        var user = refreshToken.User ?? await userRepository.GetByIdAsync(refreshToken.UserId);
        if (user is null)
            return Errors.Authentication.InvalidRefreshToken;

        refreshToken.IsRevoked = true;
        await refreshTokenRepository.DeactivateAsync(refreshToken.Id);

        var newAccessToken = jwtTokenGenerator.GenerateToken(user);
        var newRefreshToken = refreshTokenGenerator.GenerateRefreshToken(user);
        await refreshTokenRepository.AddAsync(newRefreshToken);
        
        return new AuthenticationResult(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken.Token,
            User: user.ToReturnAuthUserDto()
        );
    }
}
