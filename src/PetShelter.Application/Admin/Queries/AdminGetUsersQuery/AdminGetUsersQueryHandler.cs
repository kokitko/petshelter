using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Common.Models;
using PetShelter.Application.Dtos.Users;
using PetShelter.Application.Mappings;

namespace PetShelter.Application.Admin.Queries.AdminGetUsersQuery;

public class AdminGetUsersQueryHandler(
    IUserProfileRepository userRepository
) : IRequestHandler<AdminGetUsersQuery, ErrorOr<PagedList<ReturnAppUserDto>>>
{
    public async Task<ErrorOr<PagedList<ReturnAppUserDto>>> Handle(AdminGetUsersQuery request, CancellationToken cancellationToken)
    {
        var (users, totalCount) = await userRepository.GetUserProfilesAsync(
            request.PageNumber,
            request.PageSize,
            request.FirstName,
            request.LastName);

        List<ReturnAppUserDto> userDtos = users
            .Select(o => o.User.ToReturnAppUserDto())
            .ToList();

        return new PagedList<ReturnAppUserDto>(
            userDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
