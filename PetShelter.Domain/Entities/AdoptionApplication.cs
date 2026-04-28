using PetShelter.Domain.Common.Models;

namespace PetShelter.Domain.Entities;

public class AdoptionApplication : BaseEntity
{
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public string Message { get; set; } = string.Empty;
    
    public Guid ApplicantId { get; set; }
    public AppUser Applicant { get; set; } = null!;
    
    public Guid PetId { get; set; }
    public Pet Pet { get; set; } = null!;
}

public enum ApplicationStatus
{
    Pending,
    Approved,
    Rejected
}