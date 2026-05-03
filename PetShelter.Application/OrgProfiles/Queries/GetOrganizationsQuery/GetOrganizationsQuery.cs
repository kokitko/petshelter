using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

public record GetOrganizationsQuery(
    string? OrgName,
    string? Address,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<ReturnAppUserDto>>>;