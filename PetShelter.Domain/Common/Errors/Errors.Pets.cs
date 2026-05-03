using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class Pets
    {
        public static Error NotConfirmed => Error.Conflict(
            code: "Pets.NotConfirmed",
            description: "This pet has not been confirmed for adoption yet."
        );
        public static Error NotFound => Error.NotFound(
            code: "Pets.NotFound",
            description: "The specified pet was not found."
        );
        public static Error TooManyImages => Error.Validation(
            code: "Pets.TooManyImages",
            description: "A pet cannot have more than 10 images."
        );
        public static Error NotAvailableForAdoption => Error.Conflict(
            code: "Pets.NotAvailableForAdoption",
            description: "This pet is not available for adoption."
        );
    }
}
