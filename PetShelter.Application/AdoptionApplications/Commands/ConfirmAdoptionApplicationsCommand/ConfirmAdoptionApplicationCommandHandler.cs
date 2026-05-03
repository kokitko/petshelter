using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Commands.ConfirmAdoptionApplicationsCommand;

public class ConfirmAdoptionApplicationCommandHandler(
    IAdoptionApplicationRepository adoptionApplicationRepository,
    ICurrentUserProvider currentUserProvider,
    IPetRepository petRepository
) : IRequestHandler<ConfirmAdoptionApplicationCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ConfirmAdoptionApplicationCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserProvider.GetCurrentUserId();

        var application = await adoptionApplicationRepository.GetByIdAsync(request.Id);
        if (application is null)
            return Errors.AdoptionApplications.NotFound;
        
        if (application.Pet.OwnerId != currentUserId)
            return Errors.Authentication.Forbidden;

        if (application.Status != ApplicationStatus.Pending)
            return Errors.AdoptionApplications.InvalidStatus;

        if (application.Pet.Status != PetStatus.Available)
            return Errors.AdoptionApplications.PetNotAvailable;

        application.Status = ApplicationStatus.Approved;
        await adoptionApplicationRepository.SaveChangesAsync();

        application.Pet.Status = PetStatus.Pending;
        await petRepository.SaveChangesAsync();

        return true;
    }
}
