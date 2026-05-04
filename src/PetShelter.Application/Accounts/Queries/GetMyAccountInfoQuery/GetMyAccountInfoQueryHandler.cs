using ErrorOr;
using MediatR;
using PetShelter.Application.Accounts.Common;
using PetShelter.Application.Common.Interfaces.Authentication;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Domain.Common.Errors;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Accounts.Queries.GetMyAccountInfoQuery;

public class GetMyAccountInfoQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IAppUserRepository userRepository
) : IRequestHandler<GetMyAccountInfoQuery, ErrorOr<MyAccountInfoDto>>
{
    public async Task<ErrorOr<MyAccountInfoDto>> Handle(GetMyAccountInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserProvider.GetCurrentUserId();
        if (userId == null)
            return Errors.Authentication.Unauthenticated;

        var user = await userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            return Errors.UserProfile.NotFound;

        if (user.Role == UserRole.Organization)
        {
            MyAccountInfoDto orgAccountInfo = new MyAccountInfoDto(
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.ProfilePictureUrl,
                user.Role.ToString(),
                user.PetsCount,
                user.ApplicationsCount,
                null,
                null,
                user.OrgProfile!.OrgName,
                user.OrgProfile!.Address,
                user.OrgProfile!.Website,
                user.OrgProfile!.IsVerified
            );
            return orgAccountInfo;
        }
        else
        {
            MyAccountInfoDto userAccountInfo = new MyAccountInfoDto(
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.ProfilePictureUrl,
                user.Role.ToString(),
                user.PetsCount,
                user.ApplicationsCount,
                user.UserProfile!.FirstName,
                user.UserProfile!.LastName,
                null,
                null,
                null,
                null
            );
            return userAccountInfo;
        }
    }
}
