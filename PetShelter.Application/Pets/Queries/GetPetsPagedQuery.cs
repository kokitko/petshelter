using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Queries;

public record GetPetsPagedQuery(
    int PageNumber,
    int PageSize,
    string? Species
) : IRequest<PagedList<ReturnPetDto>>;