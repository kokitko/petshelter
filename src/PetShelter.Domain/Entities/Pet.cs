using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class Pet : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Description { get; set; } = string.Empty;
    public PetStatus Status { get; set; } = PetStatus.Available;
    public Guid OwnerId { get; set; }
    public AppUser Owner { get; set; } = null!;
    public ICollection<PetImage> Images { get; set; } = new List<PetImage>();
    public ICollection<AdoptionApplication> Applications { get; set; } = new List<AdoptionApplication>();
}

public enum PetStatus
{
    Available,
    Adopted,
    Pending
}