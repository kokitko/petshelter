using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Authentication;

public interface IRefreshTokenGenerator
{
    RefreshToken GenerateRefreshToken(AppUser user);
}
