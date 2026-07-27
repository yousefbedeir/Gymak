using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class DietPlanRepository : IDietPlanRepository
{
    private readonly AppDbContext _context;

    public DietPlanRepository(AppDbContext context) => _context = context;

    public async Task<DietPlan?> GetByIdAsync(Guid dietPlanId, CancellationToken cancellationToken = default)
        => await _context.DietPlans
            .Include(dp => dp.Trainer)
            .Include(dp => dp.Member)
            .Include(dp => dp.MealItems)
                .ThenInclude(mi => mi.FoodItem)
            .FirstOrDefaultAsync(dp => dp.DietPlanId == dietPlanId, cancellationToken);

    public async Task<IReadOnlyList<DietPlan>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
        => await _context.DietPlans
            .AsNoTracking()
            .Include(dp => dp.Trainer)
            .Include(dp => dp.MealItems)
                .ThenInclude(mi => mi.FoodItem)
            .Where(dp => dp.MemberId == memberId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<DietPlan>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default)
        => await _context.DietPlans
            .AsNoTracking()
            .Include(dp => dp.Member)
            .Include(dp => dp.MealItems)
                .ThenInclude(mi => mi.FoodItem)
            .Where(dp => dp.TrainerId == trainerId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(DietPlan dietPlan, CancellationToken cancellationToken = default)
        => await _context.DietPlans.AddAsync(dietPlan, cancellationToken);

    public void Update(DietPlan dietPlan) => _context.DietPlans.Update(dietPlan);

    public void Delete(DietPlan dietPlan) => _context.DietPlans.Remove(dietPlan);
}
