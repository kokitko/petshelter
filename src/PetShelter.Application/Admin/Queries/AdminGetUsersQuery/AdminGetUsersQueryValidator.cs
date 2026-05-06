using FluentValidation;

namespace PetShelter.Application.Admin.Queries.AdminGetUsersQuery;

public class AdminGetUsersQueryValidator : AbstractValidator<AdminGetUsersQuery>
{
    public AdminGetUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must be less than or equal to 100.");
        RuleFor(x => x.FirstName).MaximumLength(100).WithMessage("First name must be at most 100 characters.");
        RuleFor(x => x.LastName).MaximumLength(100).WithMessage("Last name must be at most 100 characters.");
    }
}
