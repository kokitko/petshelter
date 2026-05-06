using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Interfaces.Services;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Admin.Commands.AdminDeletePetCommand;

public class AdminDeletePetCommandHandler(
    IPetRepository petRepository,
    IAdoptionApplicationRepository adoptionRepository,
    ICacheService cacheService
) : IRequestHandler<AdminDeletePetCommand, ErrorOr<ChangeSensitiveInfoDto>>
{
    public async Task<ErrorOr<ChangeSensitiveInfoDto>> Handle(AdminDeletePetCommand request, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetByIdAsync(Guid.Parse(request.PetId));
        if (pet == null)
            return Errors.Pets.NotFound;

        var petId = pet.Id.ToString();

        await adoptionRepository.DeleteByPetIdAsync(pet.Id);
        await petRepository.DeleteAsync(pet);
        await cacheService.RemoveByPrefixAsync("GetPetsQuery", cancellationToken);

        return new ChangeSensitiveInfoDto(petId);
    }
}
