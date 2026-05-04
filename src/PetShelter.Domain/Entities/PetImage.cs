using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class PetImage : BaseEntity
{
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; } = false;
    public Guid PetId { get; set; }
    public Pet Pet { get; set; } = null!;
}
