using FluentValidation;

namespace PetShelter.Application.Pets.Commands.ConfirmPetCommand;

public class ConfirmPetCommandValidator : AbstractValidator<ConfirmPetCommand>
{
    public ConfirmPetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Pet ID is required.");
    }
}
