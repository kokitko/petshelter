using Microsoft.EntityFrameworkCore;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Entities;

namespace PetShelter.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(PetShelterDbContext context) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken)
    {
        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid tokenId)
    {
        var refreshToken = await context.RefreshTokens.FindAsync(tokenId);
        if (refreshToken is not null)
        {
            refreshToken.IsRevoked = true;
            await context.SaveChangesAsync();
        }
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }
}
