using FluentValidation;

namespace PetShelter.Application.OrgProfiles.Commands.UpdateOrgProfile;

public class OrgProfileUpdateCommandValidator : AbstractValidator<OrgProfileUpdateCommand>
{
    public OrgProfileUpdateCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in E.164 format.");

        RuleFor(x => x.ProfilePicture)
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .When(x => x.ProfilePicture != null)
            .WithMessage("Profile picture must be an image file.");

        RuleFor(x => x.OrgName)
            .NotEmpty()
            .WithMessage("Organization name is required.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.");
    }
}
