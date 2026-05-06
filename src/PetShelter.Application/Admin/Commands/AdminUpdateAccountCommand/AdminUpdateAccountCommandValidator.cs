using FluentValidation;

namespace PetShelter.Application.Admin.Commands.AdminUpdateAccountCommand;

public class AdminUpdateAccountCommandValidator : AbstractValidator<AdminUpdateAccountCommand>
{
    public AdminUpdateAccountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .MinimumLength(6).When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("PhoneNumber must be a valid international phone number.");

        RuleFor(x => x.ProfilePicture)
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .When(x => x.ProfilePicture != null)
            .WithMessage("Profile picture must be an image file.");

        RuleFor(x => x.OrgName)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.OrgName))
            .WithMessage("OrgName cannot be empty if provided.");

        RuleFor(x => x.Address)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage("Address cannot be empty if provided.");

        RuleFor(x => x.Website)
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage("Website must be a valid URL.");

        RuleFor(x => x.FirstName)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.FirstName))
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.LastName))
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");
    }
}
