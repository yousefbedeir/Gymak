using FluentValidation;
using Gymak.Application.DTOs;

namespace Gymak.Application.Validators;

public class CreateMuscleGroupRequestValidator : AbstractValidator<CreateMuscleGroupRequest>
{
    public CreateMuscleGroupRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Muscle group name is required.")
            .MaximumLength(50).WithMessage("Muscle group name must not exceed 50 characters.");
    }
}

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.MuscleGroupId)
            .NotEmpty().WithMessage("MuscleGroupId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Exercise name is required.")
            .MaximumLength(100).WithMessage("Exercise name must not exceed 100 characters.");

        RuleFor(x => x.ExerciseType)
            .IsInEnum().WithMessage("Select a valid exercise type.");
    }
}

public class CreateWorkoutPlanRequestValidator : AbstractValidator<CreateWorkoutPlanRequest>
{
    public CreateWorkoutPlanRequestValidator()
    {
        RuleFor(x => x.TrainerId).NotEmpty().WithMessage("TrainerId is required.");
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("MemberId is required.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Workout plan title is required.")
            .MaximumLength(150).WithMessage("Title must not exceed 150 characters.");
    }
}

public class CreateWorkoutLogRequestValidator : AbstractValidator<CreateWorkoutLogRequest>
{
    public CreateWorkoutLogRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.ExerciseId).NotEmpty().WithMessage("ExerciseId is required.");
        RuleFor(x => x.SetsCompleted).GreaterThan(0).WithMessage("Sets completed must be greater than 0.");
        RuleFor(x => x.RepsCompleted).GreaterThan(0).WithMessage("Reps completed must be greater than 0.");
        RuleFor(x => x.WeightUsed).GreaterThanOrEqualTo(0).WithMessage("Weight used cannot be negative.");
    }
}
