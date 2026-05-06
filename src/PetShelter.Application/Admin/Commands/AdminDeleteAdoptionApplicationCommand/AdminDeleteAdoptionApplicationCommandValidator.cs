using FluentValidation;

namespace PetShelter.Application.Admin.Commands.AdminDeleteAdoptionApplicationCommand;

public class AdminDeleteAdoptionApplicationCommandValidator : AbstractValidator<AdminDeleteAdoptionApplicationCommand>
{
    public AdminDeleteAdoptionApplicationCommandValidator()
    {
        RuleFor(x => x.AdoptionApplicationId)
            .NotEmpty().WithMessage("Adoption application ID is required.");
    }
}
