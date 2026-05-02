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
        var (pets, totalCount) = await repository.GetPagedAsync(
            request.Page, request.PageSize, request.Species, request.Breed, request.Name, request.Age, null);

        var dtos = pets.Select(p => p.ToPetDto()).ToList();

        return new PagedList<PetDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
