using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class Accounts
    {
        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "The user was not found.");
    }
}
