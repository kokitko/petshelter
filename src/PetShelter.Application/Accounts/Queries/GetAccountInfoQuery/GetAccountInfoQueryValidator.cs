using FluentValidation;

namespace PetShelter.Application.Accounts.Queries.GetAccountInfoQuery;

public class GetAccountInfoQueryValidator : AbstractValidator<GetAccountInfoQuery>
{
    public GetAccountInfoQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
