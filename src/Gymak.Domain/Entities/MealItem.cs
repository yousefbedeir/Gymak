using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class MealItem
{
    public Guid MealItemId { get; set; } = Guid.NewGuid();
    public Guid DietPlanId { get; set; }
    public Guid FoodItemId { get; set; }
    public MealType MealType { get; set; }
    public decimal QuantityInGrams { get; set; }

    // Navigation Properties
    public DietPlan DietPlan { get; set; } = null!;
    public FoodItem FoodItem { get; set; } = null!;
}
