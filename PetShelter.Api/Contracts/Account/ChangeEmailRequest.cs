namespace PetShelter.Api.Contracts.Account;

public record ChangeEmailRequest(
    string NewEmail,
    string CurrentPassword
);