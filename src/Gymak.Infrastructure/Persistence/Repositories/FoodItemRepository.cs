using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class FoodItemRepository : IFoodItemRepository
{
    private readonly AppDbContext _context;

    public FoodItemRepository(AppDbContext context) => _context = context;

    public async Task<FoodItem?> GetByIdAsync(Guid foodItemId, CancellationToken cancellationToken = default)
        => await _context.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == foodItemId, cancellationToken);

    public async Task<IReadOnlyList<FoodItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.FoodItems.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<FoodItem>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
        => await _context.FoodItems
            .AsNoTracking()
            .Where(f => f.Name.Contains(searchTerm))
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FoodItem foodItem, CancellationToken cancellationToken = default)
        => await _context.FoodItems.AddAsync(foodItem, cancellationToken);

    public void Update(FoodItem foodItem) => _context.FoodItems.Update(foodItem);

    public void Delete(FoodItem foodItem) => _context.FoodItems.Remove(foodItem);
}
