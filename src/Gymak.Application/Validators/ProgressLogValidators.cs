using FluentValidation;
using Gymak.Application.DTOs;

namespace Gymak.Application.Validators;

public class CreateProgressLogRequestValidator : AbstractValidator<CreateProgressLogRequest>
{
    public CreateProgressLogRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0 kg.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0 cm.");

        RuleFor(x => x.BodyFatPercentage)
            .InclusiveBetween(0, 100).When(x => x.BodyFatPercentage.HasValue)
            .WithMessage("Body fat percentage must be between 0 and 100%.");

        RuleFor(x => x.MuscleMassPercentage)
            .InclusiveBetween(0, 100).When(x => x.MuscleMassPercentage.HasValue)
            .WithMessage("Muscle mass percentage must be between 0 and 100%.");
    }
}

public class UpdateProgressLogRequestValidator : AbstractValidator<UpdateProgressLogRequest>
{
    public UpdateProgressLogRequestValidator()
    {
        RuleFor(x => x.LogId)
            .NotEmpty().WithMessage("LogId is required.");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0 kg.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0 cm.");

        RuleFor(x => x.BodyFatPercentage)
            .InclusiveBetween(0, 100).When(x => x.BodyFatPercentage.HasValue)
            .WithMessage("Body fat percentage must be between 0 and 100%.");

        RuleFor(x => x.MuscleMassPercentage)
            .InclusiveBetween(0, 100).When(x => x.MuscleMassPercentage.HasValue)
            .WithMessage("Muscle mass percentage must be between 0 and 100%.");
    }
}
