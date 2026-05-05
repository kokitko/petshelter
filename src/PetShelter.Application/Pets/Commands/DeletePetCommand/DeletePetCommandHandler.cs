using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Pets.Common;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Pets.Commands.DeletePetCommand;

public class DeletePetCommandHandler(
    IPetRepository petRepository,
    IAdoptionApplicationRepository adoptionRepository,
    ICurrentUserProvider currentUserProvider,
    ICacheService cacheService
) : IRequestHandler<DeletePetCommand, ErrorOr<DeletePetDto>>
{
    public async Task<ErrorOr<DeletePetDto>> Handle(DeletePetCommand request, CancellationToken cancellationToken)
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

        var petId = pet.Id.ToString();

        await adoptionRepository.DeleteByPetIdAsync(pet.Id);
        await petRepository.DeleteAsync(pet);
        await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);

        return new DeletePetDto(petId);
    }
}
