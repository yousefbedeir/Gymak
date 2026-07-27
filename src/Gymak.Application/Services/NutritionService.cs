using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class NutritionService : INutritionService
{
    private readonly IFoodItemRepository _foodItemRepository;
    private readonly IDietPlanRepository _dietPlanRepository;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateFoodItemRequest> _foodItemValidator;
    private readonly IValidator<CreateDietPlanRequest> _dietPlanValidator;
    private readonly IValidator<AddMealItemRequest> _mealItemValidator;

    public NutritionService(
        IFoodItemRepository foodItemRepository,
        IDietPlanRepository dietPlanRepository,
        IApplicationDbContext context,
        IValidator<CreateFoodItemRequest> foodItemValidator,
        IValidator<CreateDietPlanRequest> dietPlanValidator,
        IValidator<AddMealItemRequest> mealItemValidator)
    {
        _foodItemRepository = foodItemRepository;
        _dietPlanRepository = dietPlanRepository;
        _context = context;
        _foodItemValidator = foodItemValidator;
        _dietPlanValidator = dietPlanValidator;
        _mealItemValidator = mealItemValidator;
    }

    // ── Food Items ────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<FoodItemDto>> GetAllFoodItemsAsync(CancellationToken cancellationToken = default)
    {
        var items = await _foodItemRepository.GetAllAsync(cancellationToken);
        return items.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<FoodItemDto>> SearchFoodItemsAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        var items = await _foodItemRepository.SearchByNameAsync(searchTerm, cancellationToken);
        return items.Select(MapToDto).ToList();
    }

    public async Task<Guid> CreateFoodItemAsync(CreateFoodItemRequest request, CancellationToken cancellationToken = default)
    {
        await _foodItemValidator.ValidateAndThrowAsync(request, cancellationToken);

        var item = new FoodItem
        {
            Name = request.Name,
            CaloriesPer100g = request.CaloriesPer100g,
            ProteinPer100g = request.ProteinPer100g,
            CarbsPer100g = request.CarbsPer100g,
            FatPer100g = request.FatPer100g,
            FiberPer100g = request.FiberPer100g
        };

        await _foodItemRepository.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return item.FoodItemId;
    }

    // ── Diet Plans ────────────────────────────────────────────────────────────

    public async Task<DietPlanDto?> GetDietPlanByIdAsync(Guid dietPlanId, CancellationToken cancellationToken = default)
    {
        var plan = await _dietPlanRepository.GetByIdAsync(dietPlanId, cancellationToken);
        return plan is null ? null : MapToDto(plan);
    }

    public async Task<IReadOnlyList<DietPlanDto>> GetDietPlansByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var plans = await _dietPlanRepository.GetByMemberIdAsync(memberId, cancellationToken);
        return plans.Select(MapToDto).ToList();
    }

    public async Task<Guid> CreateDietPlanAsync(CreateDietPlanRequest request, CancellationToken cancellationToken = default)
    {
        await _dietPlanValidator.ValidateAndThrowAsync(request, cancellationToken);

        var plan = new DietPlan
        {
            TrainerId = request.TrainerId,
            MemberId = request.MemberId,
            Title = request.Title,
            Description = request.Description,
            DailyCalorieGoal = request.DailyCalorieGoal,
            DailyProteinGoal = request.DailyProteinGoal,
            DailyCarbsGoal = request.DailyCarbsGoal,
            DailyFatGoal = request.DailyFatGoal
        };

        foreach (var mi in request.MealItems)
        {
            plan.MealItems.Add(new MealItem
            {
                FoodItemId = mi.FoodItemId,
                MealType = mi.MealType,
                QuantityInGrams = mi.QuantityInGrams
            });
        }

        await _dietPlanRepository.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return plan.DietPlanId;
    }

    public async Task AddMealItemAsync(Guid dietPlanId, AddMealItemRequest request, CancellationToken cancellationToken = default)
    {
        await _mealItemValidator.ValidateAndThrowAsync(request, cancellationToken);

        var plan = await _dietPlanRepository.GetByIdAsync(dietPlanId, cancellationToken)
            ?? throw new InvalidOperationException($"Diet plan with ID '{dietPlanId}' not found.");

        plan.MealItems.Add(new MealItem
        {
            FoodItemId = request.FoodItemId,
            MealType = request.MealType,
            QuantityInGrams = request.QuantityInGrams
        });

        await _context.SaveChangesAsync(cancellationToken);
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static FoodItemDto MapToDto(FoodItem f) =>
        new(f.FoodItemId, f.Name, f.CaloriesPer100g, f.ProteinPer100g,
            f.CarbsPer100g, f.FatPer100g, f.FiberPer100g);

    private static MealItemDto MapMealItemToDto(MealItem mi)
    {
        var factor = mi.QuantityInGrams / 100m;
        return new MealItemDto(
            mi.MealItemId,
            mi.FoodItemId,
            mi.FoodItem?.Name ?? string.Empty,
            mi.MealType,
            mi.QuantityInGrams,
            TotalCalories: Math.Round((mi.FoodItem?.CaloriesPer100g ?? 0) * factor, 2),
            TotalProtein:  Math.Round((mi.FoodItem?.ProteinPer100g ?? 0) * factor, 2),
            TotalCarbs:    Math.Round((mi.FoodItem?.CarbsPer100g ?? 0) * factor, 2),
            TotalFat:      Math.Round((mi.FoodItem?.FatPer100g ?? 0) * factor, 2)
        );
    }

    private static DietPlanDto MapToDto(DietPlan plan) =>
        new(
            plan.DietPlanId,
            plan.TrainerId,
            plan.Trainer?.FullName ?? string.Empty,
            plan.MemberId,
            plan.Member?.FullName ?? string.Empty,
            plan.Title,
            plan.Description,
            plan.DailyCalorieGoal,
            plan.DailyProteinGoal,
            plan.DailyCarbsGoal,
            plan.DailyFatGoal,
            plan.MealItems.Select(MapMealItemToDto).ToList()
        );
}
