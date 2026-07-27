using FluentValidation;
using Gymak.Application.DTOs;

namespace Gymak.Application.Validators;

public class CreateSubscriptionRequestValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public CreateSubscriptionRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.PlanId)
            .NotEmpty().WithMessage("PlanId is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("StartDate is required.");
    }
}
