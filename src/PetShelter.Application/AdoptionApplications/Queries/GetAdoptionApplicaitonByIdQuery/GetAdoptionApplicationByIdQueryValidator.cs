using FluentValidation;

namespace PetShelter.Application.AdoptionApplications.Queries.GetAdoptionApplicaitonByIdQuery;

public class GetAdoptionApplicationByIdQueryValidator : AbstractValidator<GetAdoptionApplicationByIdQuery>
{
    public GetAdoptionApplicationByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Adoption application ID is required.");
    }
}
