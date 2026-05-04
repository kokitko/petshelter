using FluentValidation;

namespace PetShelter.Application.Pets.Commands.CreatePetCommand;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Species)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Breed)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.MainPicture)
            .NotNull()
            .Must(file => file.Length > 0)
            .WithMessage("Main picture must not be empty.")
            .Must(file => file.Length <= 5 * 1024 * 1024)
            .WithMessage("Main picture must be less than or equal to 5MB.")
            .Must(file => file.ContentType.StartsWith("image/"))
            .WithMessage("Main picture must be an image.");

        RuleFor(x => x.PicturesToAdd)
            .Must(pictures => pictures != null && pictures.Count <= 10)
            .WithMessage("No more than 10 pictures are allowed.")
            .ForEach(picture =>
            {
                picture.Must(file => file.Length > 0)
                    .WithMessage("Each picture must not be empty.")
                    .Must(file => file.Length <= 5 * 1024 * 1024)
                    .WithMessage("Each picture must be less than or equal to 5MB.")
                    .Must(file => file.ContentType.StartsWith("image/"))
                    .WithMessage("Each file must be an image.");
            })
            .When(x => x.PicturesToAdd != null);
    }
}
