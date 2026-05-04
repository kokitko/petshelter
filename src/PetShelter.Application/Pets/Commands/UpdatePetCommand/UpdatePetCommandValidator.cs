using FluentValidation;

namespace PetShelter.Application.Pets.Commands.UpdatePetCommand;

public class UpdatePetCommandValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Pet ID must not be empty.");

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
            .Must(file => file == null || (
                file.Length > 0 && file.Length <= 5 * 1024 * 1024 && 
                file.ContentType.StartsWith("image/"))
            ).WithMessage("Main picture must be an image and less than or equal to 5MB.");


        RuleFor(x => x.PicturesToAdd)
            .Must(pictures => pictures != null && pictures.Count <= 10)
            .WithMessage("No more than 10 pictures are allowed.")
            .ForEach(picture =>
            {
                picture.Must(file => file == null || (
                    file.Length > 0 && 
                    file.Length <= 5 * 1024 * 1024 && 
                    file.ContentType.StartsWith("image/"))
                ).WithMessage("Each picture must be an image and less than or equal to 5MB.");
            })
            .When(x => x.PicturesToAdd != null);

        RuleFor(x => x.PictureIdsToRemove)
            .Must(ids => ids == null || ids.All(id => id != Guid.Empty))
            .WithMessage("Picture IDs to remove must be valid GUIDs.");
    }
}
