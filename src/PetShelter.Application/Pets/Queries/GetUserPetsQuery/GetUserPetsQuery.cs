using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Queries.GetUserPetsQuery;

public record GetUserPetsQuery(
    Guid UserId,
    string? Name = null,
    string? Species = null,
    string? Breed = null,
    int? Age = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<PetDto>>>;
