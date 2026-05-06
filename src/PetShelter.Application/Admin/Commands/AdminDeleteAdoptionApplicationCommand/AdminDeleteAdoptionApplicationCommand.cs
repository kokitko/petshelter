using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Admin.Commands.AdminDeleteAdoptionApplicationCommand;

public record AdminDeleteAdoptionApplicationCommand(
    string AdoptionApplicationId
) : IRequest<ErrorOr<ChangeSensitiveInfoDto>>;