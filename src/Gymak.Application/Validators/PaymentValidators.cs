using FluentValidation;
using Gymak.Application.DTOs;

namespace Gymak.Application.Validators;

public class RecordPaymentRequestValidator : AbstractValidator<RecordPaymentRequest>
{
    public RecordPaymentRequestValidator()
    {
        RuleFor(x => x.SubscriptionId)
            .NotEmpty().WithMessage("SubscriptionId is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than 0.");
    }
}
