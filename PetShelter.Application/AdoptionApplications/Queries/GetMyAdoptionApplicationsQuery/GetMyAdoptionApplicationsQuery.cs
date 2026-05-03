using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.OrgProfiles.Common;

namespace PetShelter.Application.AdoptionApplications.Queries.GetMyAdoptionApplicationsQuery;

public record GetMyAdoptionApplicationsQuery(
    string? Status,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ErrorOr<PagedList<ReturnAdoptionApplicationDto>>>;