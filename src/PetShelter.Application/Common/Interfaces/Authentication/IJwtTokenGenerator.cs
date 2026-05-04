using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppUser user);
}
