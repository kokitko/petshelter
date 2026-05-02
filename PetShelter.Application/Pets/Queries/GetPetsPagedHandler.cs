using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Mappings;
using ErrorOr;

namespace PetShelter.Application.Pets.Queries;

public class GetPetsPagedHandler(IPetRepository repository) : IRequestHandler<GetPetsPagedQuery, ErrorOr<PagedList<PetDto>>>
{
    public async Task<ErrorOr<PagedList<PetDto>>> Handle(GetPetsPagedQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetPagedAsync(
            request.PageNumber, request.PageSize, request.Species);

        var dtos = items.Select(p => p.ToPetDto()).ToList();

        return new PagedList<PetDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
