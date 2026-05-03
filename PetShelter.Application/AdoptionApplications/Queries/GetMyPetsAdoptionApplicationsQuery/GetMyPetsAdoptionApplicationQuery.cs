using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Models;
using PetShelter.Application.OrgProfiles.Common;

namespace PetShelter.Application.AdoptionApplications.Queries.GetMyPetsAdoptionApplicationsQuery;

public record GetMyPetsAdoptionApplicationQuery(
    string? Status,
    int PageNumber,
    int PageSize
) : IRequest<ErrorOr<PagedList<ReturnAdoptionApplicationDto>>>;