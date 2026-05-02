using ErrorOr;
using MediatR;
using PetShelter.Application.Common.Interfaces.Persistence;
using PetShelter.Application.Dtos.Users;
using PetShelter.Domain.Common.Errors;

namespace PetShelter.Application.UserProfiles.Queries.GetUserProfileQuery;

public class GetUserProfileQueryHandler(
    IUserProfileRepository userProfileRepository
) : IRequestHandler<GetUserProfileQuery, ErrorOr<ReturnAppUserDto>>
{
    public async Task<ErrorOr<ReturnAppUserDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userProfile = await userProfileRepository.GetByIdAsync(request.UserId);
        if (userProfile == null)
            return Errors.UserProfile.NotFound;

        var userProfileDto = new ReturnAppUserDto(
            Id: userProfile.Id,
            Email: userProfile.User.Email,
            PhoneNumber: userProfile.User.PhoneNumber,
            ProfilePictureUrl: userProfile.User.ProfilePictureUrl,
            Role: userProfile.User.Role.ToString(),
            UserProfile: new ReturnUserProfileInfo(
                FirstName: userProfile.FirstName,
                LastName: userProfile.LastName
            ),
            null
        );

        return userProfileDto;
    }
}
