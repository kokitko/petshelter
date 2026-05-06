using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Admin.Queries.AdminGetUsersQuery;

public record AdminGetUsersQuery(
    string? FirstName,
    string? LastName,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<ReturnAppUserDto>>>;