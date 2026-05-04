using FluentValidation;

namespace PetShelter.Application.AdoptionApplications.Commands.ConfirmAdoptionApplicationsCommand;

public class ConfirmAdoptionApplicationCommandValidator : AbstractValidator<ConfirmAdoptionApplicationCommand>
{
    public ConfirmAdoptionApplicationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Adoption application ID is required.");
    }
}
