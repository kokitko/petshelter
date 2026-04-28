using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class OrgProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string OrgName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Website { get; set; }
    public bool IsVerified { get; set; } = false;
}
