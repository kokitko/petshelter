using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.UserProfiles.Queries.GetUserPetsQuery;

public record GetUserPetsQuery(
    Guid UserId
) : IRequest<ErrorOr<List<PetDto>>>;
