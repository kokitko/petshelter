using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetShelter.Application.Common.Interfaces.Authentication;

namespace PetShelter.Infrastructure.Authentication;

public class CurrentUserProvider(
    IHttpContextAccessor httpContextAccessor,
    ILogger<CurrentUserProvider> logger) : ICurrentUserProvider
{
    public Guid? GetCurrentUserId()
    {
        logger.LogInformation("Attempting to retrieve current user ID from HTTP context");
        var idClaim = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) 
                      ?? httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)
                      ?? httpContextAccessor.HttpContext?.User?.FindFirst("sub");

        if (idClaim != null && Guid.TryParse(idClaim.Value, out var userId))
        {
            logger.LogInformation("Current user ID retrieved successfully: {UserId}", userId);
            return userId;
        }

        logger.LogWarning("No valid user ID claim found in HTTP context");
        return null;
    }
}
