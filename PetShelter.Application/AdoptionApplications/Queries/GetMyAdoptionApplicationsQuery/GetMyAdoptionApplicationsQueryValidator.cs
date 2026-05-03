using FluentValidation;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Queries.GetMyAdoptionApplicationsQuery;

public class GetMyAdoptionApplicationsQueryValidator : AbstractValidator<GetMyAdoptionApplicationsQuery>
{
    public GetMyAdoptionApplicationsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must be less than or equal to 100.");

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<ApplicationStatus>(status, true, out _))
            .WithMessage("Status must be a valid application status.");
    }
}
