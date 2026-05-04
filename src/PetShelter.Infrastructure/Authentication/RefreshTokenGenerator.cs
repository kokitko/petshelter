using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Authentication;

public class RefreshTokenGenerator(IDateTimeProvider dateTimeProvider) : IRefreshTokenGenerator
{
    public RefreshToken GenerateRefreshToken(AppUser user)
    {
        var refreshToken = 
            Convert.ToBase64String(Guid.NewGuid().ToByteArray()) + 
            Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        RefreshToken refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = dateTimeProvider.UtcNow.AddDays(14)
        };

        return refreshTokenEntity;
    }
}
