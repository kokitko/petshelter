using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Authentication;

public class RefreshTokenGenerator(
    IDateTimeProvider dateTimeProvider,
    ILogger<RefreshTokenGenerator> logger) : IRefreshTokenGenerator
{
    public RefreshToken GenerateRefreshToken(AppUser user)
    {
        logger.LogInformation("Generating refresh token for user with ID: {UserId} and email: {Email}", user.Id, user.Email);
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

        logger.LogInformation("Refresh token generated successfully for user with ID: {UserId}, token ID: {TokenId}", user.Id, refreshTokenEntity.Id);
        return refreshTokenEntity;
    }
}
