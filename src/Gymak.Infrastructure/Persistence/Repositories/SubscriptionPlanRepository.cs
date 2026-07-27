using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class SubscriptionPlanRepository : ISubscriptionPlanRepository
{
    private readonly AppDbContext _context;

    public SubscriptionPlanRepository(AppDbContext context) => _context = context;

    public async Task<SubscriptionPlan?> GetByIdAsync(Guid planId, CancellationToken cancellationToken = default)
        => await _context.SubscriptionPlans
            .FirstOrDefaultAsync(p => p.Id == planId, cancellationToken);

    public async Task<IReadOnlyList<SubscriptionPlan>> GetAllAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _context.SubscriptionPlans.AsNoTracking();
        if (activeOnly)
        {
            query = query.Where(p => p.IsActive);
        }
        return await query.OrderBy(p => p.Price).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default)
        => await _context.SubscriptionPlans.AddAsync(plan, cancellationToken);

    public void Update(SubscriptionPlan plan) => _context.SubscriptionPlans.Update(plan);

    public void Delete(SubscriptionPlan plan) => _context.SubscriptionPlans.Remove(plan);
}
