using FluentValidation;

namespace PetShelter.Application.Pets.Queries.GetPetsQuery;

public class GetPetsQueryValidator : AbstractValidator<GetPetsQuery>
{
    public GetPetsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name must be at most 100 characters long.");
        
        RuleFor(x => x.Species)
            .MaximumLength(50)
            .WithMessage("Species must be at most 50 characters long.");

        RuleFor(x => x.Breed)
            .MaximumLength(50)
            .WithMessage("Breed must be at most 50 characters long.");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Age must be greater than or equal to 0.");
    }
}
