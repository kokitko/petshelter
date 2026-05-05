using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Commands.RejectAdoptionApplicationCommand;

public class RejectAdoptionApplicationCommandHandler(
    IAdoptionApplicationRepository adoptionApplicationRepository,
    ICurrentUserProvider currentUserProvider,
    IPetRepository petRepository, 
    ICacheService cacheService
) : IRequestHandler<RejectAdoptionApplicationCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(RejectAdoptionApplicationCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();

        var application = await adoptionApplicationRepository.GetByIdAsync(request.Id);
        if (application is null)
            return Errors.AdoptionApplications.NotFound;
        
        if (application.Pet.OwnerId != currentUserId || application.ApplicantId == currentUserId)
            return Errors.Authentication.Forbidden;

        if (application.Status == ApplicationStatus.Rejected)
            return Errors.AdoptionApplications.InvalidStatus;

        if (application.Pet.Status == PetStatus.Adopted)
            return Errors.AdoptionApplications.PetNotAvailable;

        if (application.Status == ApplicationStatus.Approved)
        {
            application.Pet.Status = PetStatus.Available;
            await petRepository.SaveChangesAsync();
            await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);
        }

        application.Status = ApplicationStatus.Rejected;
        await adoptionApplicationRepository.SaveChangesAsync();

        return true;   
    }
}
