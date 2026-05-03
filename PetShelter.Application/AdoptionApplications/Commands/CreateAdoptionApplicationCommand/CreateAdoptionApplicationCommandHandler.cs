using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.OrgProfiles.Common;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Commands.CreateAdoptionApplicationCommand;

public class CreateAdoptionApplicationCommandHandler(
    ICurrentUserProvider currentUserProvider,
    IPetRepository petRepository,
    IAdoptionApplicationRepository adoptionApplicationRepository
) : IRequestHandler<CreateAdoptionApplicationCommand, ErrorOr<ReturnAdoptionApplicationDto>>
{
    public async Task<ErrorOr<ReturnAdoptionApplicationDto>> Handle(CreateAdoptionApplicationCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var pet = await petRepository.GetByIdAsync(request.PetId);
        if (pet == null)
            return Errors.Pets.NotFound;

        if (pet.OwnerId == userId)
            return Errors.AdoptionApplications.CannotApplyToOwnPet;

        if (pet.Status == PetStatus.Adopted)
            return Errors.Pets.NotAvailableForAdoption;

        var adoptionApplication = new AdoptionApplication
        {
            Id = Guid.NewGuid(),
            PetId = request.PetId,
            ApplicantId = userId.Value,
            Message = request.Message,
            Status = ApplicationStatus.Pending
        };

        await adoptionApplicationRepository.AddAsync(adoptionApplication);

        return new ReturnAdoptionApplicationDto(
            adoptionApplication.Id,
            adoptionApplication.PetId,
            adoptionApplication.ApplicantId,
            adoptionApplication.Message,
            adoptionApplication.Status
        );
    }
}
