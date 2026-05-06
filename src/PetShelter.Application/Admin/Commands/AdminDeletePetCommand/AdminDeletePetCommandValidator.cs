using FluentValidation;

namespace PetShelter.Application.Admin.Commands.AdminDeletePetCommand;

public class AdminDeletePetCommandValidator : AbstractValidator<AdminDeletePetCommand>
{
    public AdminDeletePetCommandValidator()
    {
        RuleFor(x => x.PetId)
            .NotEmpty().WithMessage("PetId is required.");
    }
}
