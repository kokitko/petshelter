using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; } = Guid.Empty;
    public AppUser User { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}
