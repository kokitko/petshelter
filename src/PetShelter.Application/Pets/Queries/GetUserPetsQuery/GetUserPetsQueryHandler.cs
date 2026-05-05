using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Pets.Queries.GetUserPetsQuery;

public class GetUserPetsQueryHandler(
    IPetRepository petRepository
) : IRequestHandler<GetUserPetsQuery, ErrorOr<PagedList<PetDto>>>
{
    public async Task<ErrorOr<PagedList<PetDto>>> Handle(GetUserPetsQuery request, CancellationToken cancellationToken)
    {
        var (pets, totalCount) = await petRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            Enum.TryParse<PetStatus>(request.Status, true, out var status) ? status : null,
            request.Species,
            request.Breed,
            request.Name,
            request.Age,
            request.UserId);

        return new PagedList<PetDto>(
            pets.Select(p => p.ToPetDto()).ToList(), 
            totalCount, 
            request.Page, 
            request.PageSize);
    }
}
