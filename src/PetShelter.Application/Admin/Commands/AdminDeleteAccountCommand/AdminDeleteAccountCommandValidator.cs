using FluentValidation;

namespace PetShelter.Application.Admin.Commands.AdminDeleteAccountCommand;

public class AdminDeleteAccountCommandValidator : AbstractValidator<AdminDeleteAccountCommand>
{
    public AdminDeleteAccountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
