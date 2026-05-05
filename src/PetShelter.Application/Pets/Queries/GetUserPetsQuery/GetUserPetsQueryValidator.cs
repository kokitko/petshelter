using FluentValidation;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.Pets.Queries.GetUserPetsQuery;

public class GetUserPetsQueryValidator : AbstractValidator<GetUserPetsQuery>
{
    public GetUserPetsQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");
        RuleFor(x => x.Status).Must(s => s == null || Enum.TryParse<PetStatus>(s, true, out _))
            .WithMessage("Status must be either 'Available', 'Adopted', or 'Pending'.");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.Age).GreaterThanOrEqualTo(0).WithMessage("Age must be a non-negative integer.");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        RuleFor(x => x.Species).MaximumLength(50).WithMessage("Species cannot exceed 50 characters.");
        RuleFor(x => x.Breed).MaximumLength(50).WithMessage("Breed cannot exceed 50 characters.");
    }
}
