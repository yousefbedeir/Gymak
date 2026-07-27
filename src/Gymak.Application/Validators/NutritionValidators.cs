using FluentValidation;
using Gymak.Application.DTOs;

namespace Gymak.Application.Validators;

public class CreateFoodItemRequestValidator : AbstractValidator<CreateFoodItemRequest>
{
    public CreateFoodItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Food item name is required.")
            .MaximumLength(100).WithMessage("Food item name must not exceed 100 characters.");

        RuleFor(x => x.CaloriesPer100g)
            .GreaterThanOrEqualTo(0).WithMessage("Calories must be 0 or greater.");

        RuleFor(x => x.ProteinPer100g)
            .GreaterThanOrEqualTo(0).WithMessage("Protein must be 0 or greater.");

        RuleFor(x => x.CarbsPer100g)
            .GreaterThanOrEqualTo(0).WithMessage("Carbs must be 0 or greater.");

        RuleFor(x => x.FatPer100g)
            .GreaterThanOrEqualTo(0).WithMessage("Fat must be 0 or greater.");

        RuleFor(x => x.FiberPer100g)
            .GreaterThanOrEqualTo(0).WithMessage("Fiber must be 0 or greater.");
    }
}

public class CreateDietPlanRequestValidator : AbstractValidator<CreateDietPlanRequest>
{
    public CreateDietPlanRequestValidator()
    {
        RuleFor(x => x.TrainerId).NotEmpty().WithMessage("TrainerId is required.");
        RuleFor(x => x.MemberId).NotEmpty().WithMessage("MemberId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Diet plan title is required.")
            .MaximumLength(150).WithMessage("Title must not exceed 150 characters.");

        RuleFor(x => x.DailyCalorieGoal)
            .GreaterThan(0).WithMessage("Daily calorie goal must be greater than 0.");

        RuleFor(x => x.DailyProteinGoal)
            .GreaterThanOrEqualTo(0).WithMessage("Daily protein goal must be 0 or greater.");

        RuleFor(x => x.DailyCarbsGoal)
            .GreaterThanOrEqualTo(0).WithMessage("Daily carbs goal must be 0 or greater.");

        RuleFor(x => x.DailyFatGoal)
            .GreaterThanOrEqualTo(0).WithMessage("Daily fat goal must be 0 or greater.");
    }
}

public class AddMealItemRequestValidator : AbstractValidator<AddMealItemRequest>
{
    public AddMealItemRequestValidator()
    {
        RuleFor(x => x.FoodItemId).NotEmpty().WithMessage("FoodItemId is required.");
        RuleFor(x => x.MealType).IsInEnum().WithMessage("Select a valid meal type.");
        RuleFor(x => x.QuantityInGrams)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0 grams.");
    }
}
