using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class UserProfile
    {
        public static Error NotFound => Error.NotFound(
            code: "UserProfile.NotFound",
            description: "User profile not found."
        );
    }
}
