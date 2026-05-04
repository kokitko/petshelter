using FluentValidation;

namespace PetShelter.Application.Pets.Queries.GetPetByIdQuery;

public class GetPetByIdQueryValidator : AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Pet ID is required.");
    }
}
