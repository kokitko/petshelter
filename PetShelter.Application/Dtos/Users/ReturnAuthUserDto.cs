namespace PetShelter.Application.Dtos.Users;

public record ReturnAuthUserDto(
    Guid Id,
    string Email,
    string? PhoneNumber,
    string Role
);
