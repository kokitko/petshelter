using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Dtos.Users;
using PetShelter.Application.Mappings;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Accounts.Queries.GetAccountInfoQuery;

public class GetAccountInfoQueryHandler(
    IAppUserRepository userRepository
) : IRequestHandler<GetAccountInfoQuery, ErrorOr<ReturnAppUserDto>>
{
    public async Task<ErrorOr<ReturnAppUserDto>> Handle(GetAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id);
        if (user == null)
            return Errors.Accounts.NotFound;

        ReturnAppUserDto userDto = user.ToReturnAppUserDto();
        return userDto;
    }
}
