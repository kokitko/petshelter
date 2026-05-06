namespace PetShelter.Api.Contracts.Admin;

public record AdminUpdateAccountRequest(
    // basic user info
    string? Email,
    string? Password,
    string? PhoneNumber,
    IFormFile? ProfilePicture,
    // user profile info
    string? FirstName,
    string? LastName,
    // org profile info
    string? OrgName,
    string? Address,
    string? Website,
    bool? IsVerified
);