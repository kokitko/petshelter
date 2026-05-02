using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class AdoptionApplications
    {
        public static Error CannotApplyToOwnPet => Error.Conflict(
            code: "AdoptionApplication.CannotApplyToOwnPet",
            description: "You cannot apply to adopt your own pet."
        );
    }
}
