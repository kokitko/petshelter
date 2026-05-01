using ErrorOr;
using MediatR;
using PetShelter.Application.Authentication.Common;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Domain.Common.Errors;
using PetShelter.Application.Mappings;

namespace PetShelter.Application.Authentication.Commands.Login;

public class LoginCommandHandler(
    IAppUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository
) : IRequestHandler<LoginCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user is null)
            return Errors.Authentication.InvalidCredentials;

        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return Errors.Authentication.InvalidCredentials;

        var accessToken = jwtTokenGenerator.GenerateToken(user);
        var refreshToken = refreshTokenGenerator.GenerateRefreshToken(user);

        await refreshTokenRepository.AddAsync(refreshToken);

        return new AuthenticationResult(accessToken, refreshToken.Token, user.ToReturnAuthUserDto());
    }
}
