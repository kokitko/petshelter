using FluentValidation;

namespace PetShelter.Application.AdoptionApplications.Commands.CreateAdoptionApplicationCommand;

public class CreateAdoptionApplicationCommandValidator : AbstractValidator<CreateAdoptionApplicationCommand>
{
    public CreateAdoptionApplicationCommandValidator()
    {
        RuleFor(x => x.PetId).NotEmpty().WithMessage("PetId is required.");
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required.")
            .MaximumLength(1000)
            .WithMessage("Message cannot exceed 1000 characters.");
    }
}
