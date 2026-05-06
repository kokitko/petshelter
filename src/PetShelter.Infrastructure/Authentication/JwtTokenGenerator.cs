using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Options;
using PetShelter.Domain.Entities;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace PetShelter.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtSettings,
    ILogger<JwtTokenGenerator> logger) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    public string GenerateToken(AppUser user)
    {
        logger.LogInformation("Generating JWT token for user with ID: {UserId} and email: {Email}", user.Id, user.Email);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            claims: claims,
            signingCredentials: signingCredentials
        );

        logger.LogInformation("JWT token generated successfully for user with ID: {UserId}", user.Id);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
