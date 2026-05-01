using FluentValidation;

namespace PetShelter.Application.UserProfiles.Commands.UpdateUserProfile;

public class UserProfileUpdateCommandValidator : AbstractValidator<UserProfileUpdateCommand>
{
    public UserProfileUpdateCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format.");

        RuleFor(x => x.ProfilePicture)
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .When(x => x.ProfilePicture != null)
            .WithMessage("Profile picture must be an image file.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");
    }
}
