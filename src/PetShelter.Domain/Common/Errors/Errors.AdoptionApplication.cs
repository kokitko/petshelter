using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class AdoptionApplications
    {
        public static Error PetNotAvailable => Error.Conflict(
            code: "AdoptionApplication.PetNotAvailable",
            description: "The pet is not available for adoption."
        );
        public static Error NotFound => Error.NotFound(
            code: "AdoptionApplication.NotFound",
            description: "Adoption application not found."
        );
        public static Error CannotDelete => Error.Conflict(
            code: "AdoptionApplication.CannotDelete",
            description: "Cannot delete application which was approved and pet is adopted."
        );
        public static Error InvalidStatus => Error.Conflict(
            code: "AdoptionApplication.InvalidStatus",
            description: "Invalid adoption application status."
        );
        public static Error CannotApplyToOwnPet => Error.Conflict(
            code: "AdoptionApplication.CannotApplyToOwnPet",
            description: "You cannot apply to adopt your own pet."
        );
    }
}
