using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Queries;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Queries.GetPetsQuery;

public record GetPetsQuery(
    string? Status,
    string? Name,
    string? Species,
    string? Breed,
    int? Age,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<PetDto>>>, ICachedQuery
{
    public string CacheKey => $"GetPetsQuery-{Status}-{Name}-{Species}-{Breed}-{Age}-{PageNumber}-{PageSize}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}