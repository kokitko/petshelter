using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Pet;

namespace PetShelter.Application.Pets.Queries.GetPetByIdQuery;

public record GetPetByIdQuery(
    Guid Id
) : IRequest<ErrorOr<PetDto>>;