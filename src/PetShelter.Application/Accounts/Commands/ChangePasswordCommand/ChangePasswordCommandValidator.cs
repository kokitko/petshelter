using FluentValidation;

namespace PetShelter.Application.Accounts.Commands.ChangePasswordCommand;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.")
            .MinimumLength(6).WithMessage("Current password must be at least 6 characters long.")
            .MaximumLength(24).WithMessage("Current password must not exceed 24 characters.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least 6 characters long.")
            .MaximumLength(24).WithMessage("New password must not exceed 24 characters.");
    }
}
