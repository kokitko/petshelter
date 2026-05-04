using FluentValidation;

namespace PetShelter.Application.Pets.Commands.DeletePetCommand;

public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
{
    public DeletePetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Pet ID is required.");
    }
}
