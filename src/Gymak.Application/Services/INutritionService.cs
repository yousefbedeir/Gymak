using Gymak.Application.DTOs;

namespace Gymak.Application.Services;

public interface INutritionService
{
    // Food Items
    Task<IReadOnlyList<FoodItemDto>> GetAllFoodItemsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FoodItemDto>> SearchFoodItemsAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<Guid> CreateFoodItemAsync(CreateFoodItemRequest request, CancellationToken cancellationToken = default);

    // Diet Plans
    Task<DietPlanDto?> GetDietPlanByIdAsync(Guid dietPlanId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DietPlanDto>> GetDietPlansByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<Guid> CreateDietPlanAsync(CreateDietPlanRequest request, CancellationToken cancellationToken = default);
    Task AddMealItemAsync(Guid dietPlanId, AddMealItemRequest request, CancellationToken cancellationToken = default);
}
