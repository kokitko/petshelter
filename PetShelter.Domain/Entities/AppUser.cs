using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class AppUser : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public UserProfile? UserProfile { get; set; }
    public OrgProfile? OrgProfile { get; set; }
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    public ICollection<AdoptionApplication> Applications { get; set; } = new List<AdoptionApplication>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

public enum UserRole
{
    Admin,
    Organization,
    User
}