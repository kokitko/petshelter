using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Admin.Queries.AdminGetOrganizationsQuery;

public record AdminGetOrganizationsQuery(
    string? OrgName,
    string? Address,
    bool? IsVerified,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<ReturnAppUserDto>>>;