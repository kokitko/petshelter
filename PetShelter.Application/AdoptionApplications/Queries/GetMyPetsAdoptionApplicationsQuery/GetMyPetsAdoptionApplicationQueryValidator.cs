using FluentValidation;
using PetShelter.Domain.Entities;

namespace PetShelter.Application.AdoptionApplications.Queries.GetMyPetsAdoptionApplicationsQuery;

public class GetMyPetsAdoptionApplicationQueryValidator : AbstractValidator<GetMyPetsAdoptionApplicationQuery>
{
    public GetMyPetsAdoptionApplicationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Page size must be less than or equal to 100.");

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<ApplicationStatus>(status, out _))
            .WithMessage("Status must be a valid application status or null/empty.");
    }
}
