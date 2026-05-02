using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.UserProfiles.Queries.GetUserProfileQuery;

public record GetUserProfileQuery(
    Guid UserId
) : IRequest<ErrorOr<ReturnAppUserDto>>;