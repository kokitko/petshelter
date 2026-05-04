using PetShelter.Domain.Entities;

namespace PetShelter.Application.Common.Interfaces.Persistence;

public interface IAdoptionApplicationRepository
{
    Task AddAsync(AdoptionApplication application);
    Task<AdoptionApplication?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
    Task DeleteAsync(AdoptionApplication application);
    Task<IEnumerable<AdoptionApplication>> GetByPetIdAsync(Guid petId);
    Task DeleteByPetIdAsync(Guid petId);
    Task<AdoptionApplication?> GetApprovedByPetIdAsync(Guid petId);
    Task<(List<AdoptionApplication>, int)> GetByApplicantIdAsync(Guid applicantId, ApplicationStatus? status, int pageNumber, int pageSize);
    Task<(List<AdoptionApplication>, int)> GetByOwnerIdAsync(Guid ownerId, ApplicationStatus? status, int pageNumber, int pageSize);
}
