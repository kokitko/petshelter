using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Admin.Commands.AdminDeleteAdoptionApplicationCommand;

public class AdminDeleteAdoptionApplicationCommandHandler(
    IAdoptionApplicationRepository adoptionRepository,
    IPetRepository petRepository,
    ICacheService cacheService
) : IRequestHandler<AdminDeleteAdoptionApplicationCommand, ErrorOr<ChangeSensitiveInfoDto>>
{
    public async Task<ErrorOr<ChangeSensitiveInfoDto>> Handle(AdminDeleteAdoptionApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await adoptionRepository.GetByIdAsync(Guid.Parse(request.AdoptionApplicationId));
        if (application == null)
            return Errors.AdoptionApplications.NotFound;
            
        var pet = await petRepository.GetByIdAsync(application.PetId);
        if (pet == null)
            return Errors.Pets.NotFound;

        if (application.Status == ApplicationStatus.Approved && pet.Status == PetStatus.Adopted)
            return Errors.AdoptionApplications.CannotDelete;

        if (application.Status == ApplicationStatus.Approved && pet.Status == PetStatus.Pending)
        {
            pet.Status = PetStatus.Available;
            await petRepository.SaveChangesAsync();
        }

        await adoptionRepository.DeleteAsync(application);
        await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);
        return new ChangeSensitiveInfoDto(application.Id.ToString());
    }
}
