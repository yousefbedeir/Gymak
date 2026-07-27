using Gymak.Domain.Entities;
using Gymak.Domain.Enums;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context) => _context = context;

    public async Task<Subscription?> GetByIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
        => await _context.Subscriptions
            .Include(s => s.User)
            .Include(s => s.Plan)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId, cancellationToken);

    public async Task<IReadOnlyList<Subscription>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Subscriptions
            .AsNoTracking()
            .Include(s => s.Plan)
            .Include(s => s.Payments)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);

    public async Task<Subscription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Subscriptions
            .Include(s => s.Plan)
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active && s.EndDate >= DateTime.UtcNow)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Subscriptions
            .AsNoTracking()
            .Include(s => s.User)
            .Include(s => s.Plan)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        => await _context.Subscriptions.AddAsync(subscription, cancellationToken);

    public void Update(Subscription subscription) => _context.Subscriptions.Update(subscription);
}
