using FluentValidation;

namespace PetShelter.Application.AdoptionApplications.Commands.RejectAdoptionApplicationCommand;

public class RejectAdoptionApplicationCommandValidator : AbstractValidator<RejectAdoptionApplicationCommand>
{
    public RejectAdoptionApplicationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Adoption application ID is required.");
    }
}
