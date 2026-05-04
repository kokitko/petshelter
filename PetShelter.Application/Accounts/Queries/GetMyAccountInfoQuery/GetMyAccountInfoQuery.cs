using ErrorOr;
using MediatR;
using PetShelter.Application.Accounts.Common;

namespace PetShelter.Application.Accounts.Queries.GetMyAccountInfoQuery;

public record GetMyAccountInfoQuery() : IRequest<ErrorOr<MyAccountInfoDto>>;