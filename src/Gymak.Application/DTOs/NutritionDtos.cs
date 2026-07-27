using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record FoodItemDto(
    Guid FoodItemId,
    string Name,
    decimal CaloriesPer100g,
    decimal ProteinPer100g,
    decimal CarbsPer100g,
    decimal FatPer100g,
    decimal FiberPer100g
);

public record CreateFoodItemRequest(
    string Name,
    decimal CaloriesPer100g,
    decimal ProteinPer100g,
    decimal CarbsPer100g,
    decimal FatPer100g,
    decimal FiberPer100g
);

public record MealItemDto(
    Guid MealItemId,
    Guid FoodItemId,
    string FoodItemName,
    MealType MealType,
    decimal QuantityInGrams,
    decimal TotalCalories,
    decimal TotalProtein,
    decimal TotalCarbs,
    decimal TotalFat
);

public record AddMealItemRequest(
    Guid FoodItemId,
    MealType MealType,
    decimal QuantityInGrams
);

public record DietPlanDto(
    Guid DietPlanId,
    Guid TrainerId,
    string TrainerName,
    Guid MemberId,
    string MemberName,
    string Title,
    string? Description,
    int DailyCalorieGoal,
    decimal DailyProteinGoal,
    decimal DailyCarbsGoal,
    decimal DailyFatGoal,
    IReadOnlyList<MealItemDto> MealItems
);

public record CreateDietPlanRequest(
    Guid TrainerId,
    Guid MemberId,
    string Title,
    string? Description,
    int DailyCalorieGoal,
    decimal DailyProteinGoal,
    decimal DailyCarbsGoal,
    decimal DailyFatGoal,
    List<AddMealItemRequest> MealItems
);
