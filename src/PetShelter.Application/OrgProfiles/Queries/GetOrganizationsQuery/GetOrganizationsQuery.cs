using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Queries;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

public record GetOrganizationsQuery(
    string? OrgName,
    string? Address,
    bool? IsVerified,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<ReturnAppUserDto>>>, ICachedQuery
{
    public string CacheKey => $"GetOrganizationsQuery-{OrgName}-{Address}-{PageNumber}-{PageSize}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}