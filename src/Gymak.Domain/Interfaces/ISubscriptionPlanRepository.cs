using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface ISubscriptionPlanRepository
{
    Task<SubscriptionPlan?> GetByIdAsync(Guid planId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SubscriptionPlan>> GetAllAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task AddAsync(SubscriptionPlan plan, CancellationToken cancellationToken = default);
    void Update(SubscriptionPlan plan);
    void Delete(SubscriptionPlan plan);
}
