namespace PetShelter.Application.Dtos.Users;

public record AppUserResponse(
    Guid Id,
    string Email,
    string Role
);
