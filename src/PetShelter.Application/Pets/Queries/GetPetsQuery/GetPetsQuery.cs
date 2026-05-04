using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Queries.GetPetsQuery;

public record GetPetsQuery(
    string? Name,
    string? Species,
    string? Breed,
    int? Age,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<PetDto>>>;