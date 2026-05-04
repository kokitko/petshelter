using FluentValidation;

namespace PetShelter.Application.Authentication.Commands.Login;

public class LoginRequestValidator : AbstractValidator<LoginCommand>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
