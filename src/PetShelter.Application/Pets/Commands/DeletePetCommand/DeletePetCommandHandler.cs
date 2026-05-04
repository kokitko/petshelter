using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Pets.Commands.DeletePetCommand;

public class DeletePetCommandHandler(
    IPetRepository petRepository,
    IAdoptionApplicationRepository adoptionRepository,
    ICurrentUserProvider currentUserProvider
) : IRequestHandler<DeletePetCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(DeletePetCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var pet = await petRepository.GetByIdAsync(request.Id);
        if (pet == null)
            return Errors.Pets.NotFound;

        if (pet.OwnerId != userId)
            return Errors.Authentication.Forbidden;

        if (pet.Status != PetStatus.Available)
            return Errors.Pets.CannotDelete;

        await adoptionRepository.DeleteByPetIdAsync(pet.Id);
        await petRepository.DeleteAsync(pet);
        return true;
    }
}
