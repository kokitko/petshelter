using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrgProfileQuery;

public record GetOrgProfileQuery(
    Guid Id
) : IRequest<ErrorOr<ReturnAppUserDto>>;