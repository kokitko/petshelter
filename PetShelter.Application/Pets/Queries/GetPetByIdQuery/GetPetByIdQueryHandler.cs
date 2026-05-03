using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.Pets.Queries.GetPetByIdQuery;

public class GetPetByIdQueryHandler(
    IPetRepository petRepository
) : IRequestHandler<GetPetByIdQuery, ErrorOr<PetDto>>
{
    public async Task<ErrorOr<PetDto>> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
    {
        var pet = await petRepository.GetByIdAsync(request.Id);

        if (pet == null)
            return Errors.Pets.NotFound;

        return pet.ToPetDto();
    }
}
