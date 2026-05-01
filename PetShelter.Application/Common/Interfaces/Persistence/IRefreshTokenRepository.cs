using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task AddAsync(RefreshToken refreshToken);
    Task DeactivateAsync(Guid tokenId);
}
