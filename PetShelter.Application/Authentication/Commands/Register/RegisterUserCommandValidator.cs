using FluentValidation;

namespace PetShelter.Application.Authentication.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(24);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in E.164 format.");

        RuleFor(x => x)
            .Must(x => x.OrgProfile is not null || x.UserProfile is not null)
            .WithMessage("Either OrgProfile or UserProfile must be provided.");

        RuleFor(x => x.OrgProfile!)
            .SetValidator(new OrgProfileRequestValidator())
            .When(x => x.OrgProfile is not null);

        RuleFor(x => x.UserProfile!)
            .SetValidator(new UserProfileRequestValidator())
            .When(x => x.UserProfile is not null);
    }

    public class UserProfileRequestValidator : AbstractValidator<UserProfileInfo>
    {
        public UserProfileRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);
        }
    }

    public class OrgProfileRequestValidator : AbstractValidator<OrgProfileInfo>
    {
        public OrgProfileRequestValidator()
        {
            RuleFor(x => x.OrgName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Website)
                .MaximumLength(200)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Website must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.Website));
        }
    }
}
