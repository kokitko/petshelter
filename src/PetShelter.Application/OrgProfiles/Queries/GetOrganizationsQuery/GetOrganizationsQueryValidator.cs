using System.Data;
using FluentValidation;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrganizationsQuery;

public class GetOrganizationsCommandValidator : AbstractValidator<GetOrganizationsQuery>
{
    public GetOrganizationsCommandValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must be less than or equal to 100.");

        RuleFor(x => x.OrgName)
            .MaximumLength(100).WithMessage("Organization name must be at most 100 characters long.");

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address must be at most 200 characters long.");
    }
}
