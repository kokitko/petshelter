using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class OrgProfile
    {
        public static Error NotFound => Error.NotFound(
            code: "OrgProfile.NotFound",
            description: "Organization profile not found."
        );
    }
}
