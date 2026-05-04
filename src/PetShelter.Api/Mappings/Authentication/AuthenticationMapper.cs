using PetShelter.Api.Contracts.Authentication;
using PetShelter.Application.Authentication.Commands;
using PetShelter.Application.Authentication.Common;
using PetShelter.Application.Dtos.Users;
using Riok.Mapperly.Abstractions;

namespace PetShelter.Api.Mappings.Authentication;

[Mapper]
public static partial class AuthenticationMapper
{
#pragma warning disable RMG020
    [MapProperty(nameof(AuthResponse.User), nameof(AuthenticationResult.User))]
    public static partial AuthResponse ToAuthResponse(this AuthenticationResult authResult);
    public static partial UserAuthResponse ToUserAuthResponse(this ReturnAuthUserDto authResult);
    [MapProperty(nameof(RegisterUserCommand.UserProfile), nameof(RegisterRequest.UserProfile))]
    [MapProperty(nameof(RegisterUserCommand.OrgProfile), nameof(RegisterRequest.OrgProfile))]
    public static partial RegisterUserCommand ToRegisterUserCommand(this RegisterRequest request);
#pragma warning restore RMG020
}
