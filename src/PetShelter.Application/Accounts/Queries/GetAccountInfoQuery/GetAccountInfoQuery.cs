using ErrorOr;
using MediatR;
using PetShelter.Application.Dtos.Users;

namespace PetShelter.Application.Accounts.Queries.GetAccountInfoQuery;

public record GetAccountInfoQuery(
    Guid Id
) : IRequest<ErrorOr<ReturnAppUserDto>>;