using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Pets.Commands.ConfirmPetCommand;

public class ConfirmPetCommandHandler(
    IPetRepository petRepository,
    IAdoptionApplicationRepository adoptionApplicationRepository,
    ICurrentUserProvider currentUserService,
    ICacheService cacheService
) : IRequestHandler<ConfirmPetCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(ConfirmPetCommand request, CancellationToken cancellationToken)
    {
        string userId = currentUserService.GetCurrentUserId()?.ToString() ?? string.Empty;

        var pet = await petRepository.GetByIdAsync(request.Id);
        if (pet == null)
            return Errors.Pets.NotFound;

        var confirmedApplication = await adoptionApplicationRepository.GetApprovedByPetIdAsync(request.Id);
        if (confirmedApplication == null)
            return Errors.Pets.NotConfirmed;
        if (confirmedApplication.ApplicantId != Guid.Parse(userId))
            return Errors.Authentication.Forbidden;

        pet.Status = PetStatus.Adopted;
        await petRepository.SaveChangesAsync();

        var applicationsToReject = (await adoptionApplicationRepository.GetByPetIdAsync(request.Id))
            .Where(app => app.Id != confirmedApplication.Id)
            .ToList();

        foreach (var application in applicationsToReject)
            application.Status = ApplicationStatus.Rejected;

        await adoptionApplicationRepository.SaveChangesAsync();
        await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);

        return true;
    }
}
