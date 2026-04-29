using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Mappings;
using ErrorOr;

namespace PetShelter.Application.Pets.Queries;

public class GetPetsPagedHandler(IPetRepository repository) : IRequestHandler<GetPetsPagedQuery, ErrorOr<PagedList<PetResponse>>>
{
    public async Task<ErrorOr<PagedList<PetResponse>>> Handle(GetPetsPagedQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetPagedAsync(
            request.PageNumber, request.PageSize, request.Species);

        var dtos = items.Select(p => p.ToPetResponse()).ToList();

        return new PagedList<PetResponse>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
