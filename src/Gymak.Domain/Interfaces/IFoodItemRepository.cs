using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IFoodItemRepository
{
    Task<FoodItem?> GetByIdAsync(Guid foodItemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FoodItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FoodItem>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task AddAsync(FoodItem foodItem, CancellationToken cancellationToken = default);
    void Update(FoodItem foodItem);
    void Delete(FoodItem foodItem);
}
