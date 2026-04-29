using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Mappings;

namespace PetShelter.Application.Pets.Queries;

public class GetPetsPagedHandler(IPetRepository repository) : IRequestHandler<GetPetsPagedQuery, PagedList<ReturnPetDto>>
{
    public async Task<PagedList<ReturnPetDto>> Handle(GetPetsPagedQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await repository.GetPagedAsync(
            request.PageNumber, request.PageSize, request.Species);

        var dtos = items.Select(p => p.ToReturnPetDto()).ToList();

        return new PagedList<ReturnPetDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
