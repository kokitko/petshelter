using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Mappings;

namespace PetShelter.Application.Pets.Queries.GetPetsQuery;

public class GetPetsQueryHandler(
    IPetRepository petRepository
) : IRequestHandler<GetPetsQuery, ErrorOr<PagedList<PetDto>>>
{
    public async Task<ErrorOr<PagedList<PetDto>>> Handle(GetPetsQuery request, CancellationToken cancellationToken)
    {
        var (pets, totalCount) = await petRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Species,
            request.Breed,
            request.Name, 
            request.Age,
            null);

        var petDtos = pets.Select(p => p.ToPetDto()).ToList();

        var pagedList = new PagedList<PetDto>(
            petDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return pagedList;
    }
}
