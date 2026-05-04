using FluentValidation;

namespace PetShelter.Application.Pets.Queries.GetUserPetsQuery;

public class GetUserPetsQueryValidator : AbstractValidator<GetUserPetsQuery>
{
    public GetUserPetsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.Age).GreaterThanOrEqualTo(0).WithMessage("Age must be a non-negative integer.");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        RuleFor(x => x.Species).MaximumLength(50).WithMessage("Species cannot exceed 50 characters.");
        RuleFor(x => x.Breed).MaximumLength(50).WithMessage("Breed cannot exceed 50 characters.");
    }
}
