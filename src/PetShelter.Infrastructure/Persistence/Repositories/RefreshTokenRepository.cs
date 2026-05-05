using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(
    PetShelterDbContext context,
    ILogger<RefreshTokenRepository> logger
    ) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken)
    {
        try {
            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();
            logger.LogInformation("Refresh token added successfully with id: {TokenId} for userId: {UserId}", refreshToken.Id, refreshToken.UserId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error adding refresh token for userId: {UserId}", refreshToken.UserId);
            throw;
        }
    }

    public async Task DeactivateAsync(Guid tokenId)
    {
        try {
            var refreshToken = await context.RefreshTokens.FindAsync(tokenId);
            if (refreshToken is not null)
            {
                refreshToken.IsRevoked = true;
                await context.SaveChangesAsync();
                logger.LogInformation("Refresh token deactivated successfully with id: {TokenId} for userId: {UserId}", refreshToken.Id, refreshToken.UserId);
            } else {
                logger.LogWarning("Attempted to deactivate non-existent refresh token with id: {TokenId}", tokenId);
            }
        } catch (Exception ex) {
            logger.LogError(ex, "Error deactivating refresh token with id: {TokenId}", tokenId);
            throw;
        }
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        try {
            var refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
            if (refreshToken != null)            {
                logger.LogInformation("Retrieved refresh token with id: {TokenId} for userId: {UserId}", refreshToken.Id, refreshToken.UserId);
                return refreshToken;
            } else {
                logger.LogWarning("No refresh token found with token: {Token}", token);
                return null;
            }
        } catch (Exception ex) {
            logger.LogError(ex, "Error retrieving refresh token with token: {Token}", token);
            throw;
        }
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        try {
            var tokens = await context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
            context.RefreshTokens.RemoveRange(tokens);
            await context.SaveChangesAsync();
            logger.LogInformation("Deleted {TokenCount} refresh tokens for userId: {UserId}", tokens.Count, userId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting refresh tokens for userId: {UserId}", userId);
            throw;
        }
    }

    public async Task DeleteAsync(RefreshToken token)
    {
        try {
            context.RefreshTokens.Remove(token);
            await context.SaveChangesAsync();
            logger.LogInformation("Refresh token deleted successfully with id: {TokenId} for userId: {UserId}", token.Id, token.UserId);
        } catch (Exception ex) {
            logger.LogError(ex, "Error deleting refresh token with id: {TokenId} for userId: {UserId}", token.Id, token.UserId);
            throw;
        }
    }
}
