using FluentValidation;
using Gymak.Application.DTOs;

namespace Gymak.Application.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Select a valid user role.");
    }
}

public class UpsertMemberProfileRequestValidator : AbstractValidator<UpsertMemberProfileRequest>
{
    public UpsertMemberProfileRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("Height must be greater than 0 cm.");

        RuleFor(x => x.CurrentWeight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0 kg.");
    }
}

public class AssignTrainerRequestValidator : AbstractValidator<AssignTrainerRequest>
{
    public AssignTrainerRequestValidator()
    {
        RuleFor(x => x.TrainerId)
            .NotEmpty().WithMessage("TrainerId is required.");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("ClientId is required.")
            .NotEqual(x => x.TrainerId).WithMessage("A trainer cannot be assigned to themselves as a client.");
    }
}
