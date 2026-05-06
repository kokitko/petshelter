using FluentValidation;

namespace PetShelter.Application.Admin.Commands.AdminUpdatePetCommand;

public class AdminUpdatePetCommandValidator : AbstractValidator<AdminUpdatePetCommand>
{
    public AdminUpdatePetCommandValidator()
    {
        RuleFor(x => x.PetId)
            .NotEmpty().WithMessage("Pet ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Species)
            .NotEmpty().WithMessage("Species is required.")
            .MaximumLength(50).WithMessage("Species cannot exceed 50 characters.");

        RuleFor(x => x.Breed)
            .MaximumLength(50).WithMessage("Breed cannot exceed 50 characters.");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(0).WithMessage("Age must be a non-negative integer.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.MainPicture)
            .Must(file => file == null || file.ContentType.StartsWith("image/"))
            .When(x => x.MainPicture != null)
            .WithMessage("Main picture must be an image file.");

        RuleForEach(x => x.PicturesToAdd)
            .Must(file => file.ContentType.StartsWith("image/"))
            .WithMessage("All pictures to add must be image files.");

        RuleForEach(x => x.PictureIdsToRemove)
            .NotEmpty().WithMessage("Picture IDs to remove cannot be empty.");
    }
}
