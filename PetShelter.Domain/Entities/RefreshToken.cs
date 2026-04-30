using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } 
    public bool IsRevoked { get; set; }

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}
