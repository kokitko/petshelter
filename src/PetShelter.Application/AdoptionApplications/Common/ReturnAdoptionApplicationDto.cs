using PetShelter.Domain.Entities;

namespace PetShelter.Application.OrgProfiles.Common;

public record ReturnAdoptionApplicationDto(
    Guid Id,
    Guid PetId,
    Guid ApplicantId,
    string Message,
    ApplicationStatus Status
);
