using Gymak.Application.DTOs;
using Gymak.Domain.Enums;

namespace Gymak.Application.Services;

public interface ISubscriptionService
{
    // Subscription Plans
    Task<IReadOnlyList<SubscriptionPlanDto>> GetAllPlansAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanDto?> GetPlanByIdAsync(Guid planId, CancellationToken cancellationToken = default);
    Task<Guid> CreatePlanAsync(CreateSubscriptionPlanRequest request, CancellationToken cancellationToken = default);
    Task UpdatePlanAsync(UpdateSubscriptionPlanRequest request, CancellationToken cancellationToken = default);
    Task DeletePlanAsync(Guid planId, CancellationToken cancellationToken = default);

    // Subscriptions
    Task<IReadOnlyList<SubscriptionDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> GetSubscriptionByIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> GetActiveSubscriptionByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> CreateSubscriptionAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken = default);
    Task UpdateSubscriptionStatusAsync(Guid subscriptionId, SubscriptionStatus status, CancellationToken cancellationToken = default);
}
