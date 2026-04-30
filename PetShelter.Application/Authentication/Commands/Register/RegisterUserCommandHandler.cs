using ErrorOr;
using MediatR;
using PetShelter.Application.Authentication.Common;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Mappings;

namespace PetShelter.Application.Authentication.Commands;

public class RegisterUserCommandHandler(
    IAppUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator
) : IRequestHandler<RegisterUserCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterUserCommand request, 
        CancellationToken cancellationToken)
    {
        if (await userRepository.GetByEmailAsync(request.Email) is not null)
            return Errors.Authentication.DuplicateEmail;

        if (request.OrgProfile != null && request.UserProfile != null)
            return Errors.Authentication.InvalidUserType;

        if (request.OrgProfile == null && request.UserProfile == null)
            return Errors.Authentication.MissingUserType;

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber,
            Role = request.UserProfile != null ? UserRole.User : UserRole.Organization
        };

        if (request.OrgProfile != null)
        {
            OrgProfile orgProfile = request.OrgProfile.ToOrgProfile();
            orgProfile.Id = Guid.NewGuid();
            orgProfile.UserId = user.Id;
            orgProfile.User = user;
            orgProfile.IsVerified = false;

            user.OrgProfile = orgProfile;
        }
        else if (request.UserProfile != null)
        {
            UserProfile userProfile = request.UserProfile.ToUserProfile();
            userProfile.Id = Guid.NewGuid();
            userProfile.UserId = user.Id;
            userProfile.User = user;

            user.UserProfile = userProfile;
        }

        var accessToken = jwtTokenGenerator.GenerateToken(user);
        var refreshTokenEntity = refreshTokenGenerator.GenerateRefreshToken(user);

        user.RefreshTokens.Add(refreshTokenEntity);

        await userRepository.AddAsync(user);

        var userDto = user.ToReturnAuthUserDto();

        return new AuthenticationResult(accessToken, refreshTokenEntity.Token, userDto);
    }
}
