using ErrorOr;

namespace PetShelter.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "Authentication.DuplicateEmail",
            description: "The email is already registered."
        );

        public static Error InvalidUserType => Error.Validation(
            code: "Authentication.InvalidUserType",
            description: "A user cannot have both an organization profile and a user profile."
        );
        
        public static Error MissingUserType => Error.Validation(
            code: "Authentication.MissingUserType",
            description: "A user must have either an organization profile or a user profile."
        );

        public static Error InvalidCredentials => Error.Unauthorized(
            code: "Authentication.InvalidCredentials",
            description: "Invalid email or password."
        );

        public static Error InvalidRefreshToken => Error.Unauthorized(
            code: "Authentication.InvalidRefreshToken",
            description: "The provided refresh token is invalid or has expired."
        );
    }
}
