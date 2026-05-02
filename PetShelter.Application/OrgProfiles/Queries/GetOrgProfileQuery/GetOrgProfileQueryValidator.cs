using FluentValidation;

namespace PetShelter.Application.OrgProfiles.Queries.GetOrgProfileQuery;

public class GetOrgProfileQueryValidator : AbstractValidator<GetOrgProfileQuery>
{
    public GetOrgProfileQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Organization ID is required.");
    }
}
