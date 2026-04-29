using PetShelter.Application.Common.Interfaces.Persistence;

namespace PetShelter.Application.Authentication.Commands;

public class RegisterUserCommandHandler(
    IAppUserRepository userRepository,
    IOrgProfileRepository orgProfileRepository,
    IUserProfileRepository userProfileRepository
)
{

}
