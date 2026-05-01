using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PetShelter.Application.Common.Interfaces.Authentication;

namespace PetShelter.Infrastructure.Authentication;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public Guid? GetCurrentUserId()
    {
        var idClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) 
                      ?? httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)
                      ?? httpContextAccessor.HttpContext?.User?.FindFirst("sub");

        if (idClaim != null && Guid.TryParse(idClaim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }
}
