using FluentValidation;
using Gymak.Application.Common.Interfaces;
using Gymak.Application.DTOs;
using Gymak.Domain.Entities;
using Gymak.Domain.Enums;
using Gymak.Domain.Interfaces;

namespace Gymak.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateSubscriptionPlanRequest> _createPlanValidator;
    private readonly IValidator<UpdateSubscriptionPlanRequest> _updatePlanValidator;
    private readonly IValidator<CreateSubscriptionRequest> _createSubscriptionValidator;

    public SubscriptionService(
        ISubscriptionPlanRepository planRepository,
        ISubscriptionRepository subscriptionRepository,
        IApplicationDbContext context,
        IValidator<CreateSubscriptionPlanRequest> createPlanValidator,
        IValidator<UpdateSubscriptionPlanRequest> updatePlanValidator,
        IValidator<CreateSubscriptionRequest> createSubscriptionValidator)
    {
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _context = context;
        _createPlanValidator = createPlanValidator;
        _updatePlanValidator = updatePlanValidator;
        _createSubscriptionValidator = createSubscriptionValidator;
    }

    // ── Plans ─────────────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<SubscriptionPlanDto>> GetAllPlansAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var plans = await _planRepository.GetAllAsync(activeOnly, cancellationToken);
        return plans.Select(MapPlanToDto).ToList();
    }

    public async Task<SubscriptionPlanDto?> GetPlanByIdAsync(Guid planId, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(planId, cancellationToken);
        return plan is null ? null : MapPlanToDto(plan);
    }

    public async Task<Guid> CreatePlanAsync(CreateSubscriptionPlanRequest request, CancellationToken cancellationToken = default)
    {
        await _createPlanValidator.ValidateAndThrowAsync(request, cancellationToken);

        var plan = new SubscriptionPlan
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            BillingCycle = request.BillingCycle,
            DurationDays = request.DurationDays,
            Features = request.Features,
            IsActive = true
        };

        await _planRepository.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return plan.Id;
    }

    public async Task UpdatePlanAsync(UpdateSubscriptionPlanRequest request, CancellationToken cancellationToken = default)
    {
        await _updatePlanValidator.ValidateAndThrowAsync(request, cancellationToken);

        var plan = await _planRepository.GetByIdAsync(request.PlanId, cancellationToken)
            ?? throw new InvalidOperationException($"Plan with ID '{request.PlanId}' was not found.");

        plan.Name = request.Name;
        plan.Description = request.Description;
        plan.Price = request.Price;
        plan.BillingCycle = request.BillingCycle;
        plan.DurationDays = request.DurationDays;
        plan.Features = request.Features;
        plan.IsActive = request.IsActive;
        plan.LastModifiedAt = DateTime.UtcNow;

        _planRepository.Update(plan);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePlanAsync(Guid planId, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(planId, cancellationToken)
            ?? throw new InvalidOperationException($"Plan with ID '{planId}' was not found.");

        _planRepository.Delete(plan);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // ── Subscriptions ─────────────────────────────────────────────────────────

    public async Task<IReadOnlyList<SubscriptionDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        var subs = await _subscriptionRepository.GetAllAsync(cancellationToken);
        return subs.Select(MapSubscriptionToDto).ToList();
    }

    public async Task<SubscriptionDto?> GetSubscriptionByIdAsync(Guid subscriptionId, CancellationToken cancellationToken = default)
    {
        var sub = await _subscriptionRepository.GetByIdAsync(subscriptionId, cancellationToken);
        return sub is null ? null : MapSubscriptionToDto(sub);
    }

    public async Task<SubscriptionDto?> GetActiveSubscriptionByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var sub = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId, cancellationToken);
        return sub is null ? null : MapSubscriptionToDto(sub);
    }

    public async Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var subs = await _subscriptionRepository.GetByUserIdAsync(userId, cancellationToken);
        return subs.Select(MapSubscriptionToDto).ToList();
    }

    public async Task<Guid> CreateSubscriptionAsync(CreateSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        await _createSubscriptionValidator.ValidateAndThrowAsync(request, cancellationToken);

        var plan = await _planRepository.GetByIdAsync(request.PlanId, cancellationToken)
            ?? throw new InvalidOperationException($"Subscription plan with ID '{request.PlanId}' not found.");

        var endDate = request.StartDate.AddDays(plan.DurationDays);

        var subscription = new Subscription
        {
            UserId = request.UserId,
            PlanId = request.PlanId,
            StartDate = request.StartDate,
            EndDate = endDate,
            AutoRenew = request.AutoRenew,
            Status = SubscriptionStatus.Active
        };

        await _subscriptionRepository.AddAsync(subscription, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return subscription.Id;
    }

    public async Task UpdateSubscriptionStatusAsync(Guid subscriptionId, SubscriptionStatus status, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(subscriptionId, cancellationToken)
            ?? throw new InvalidOperationException($"Subscription with ID '{subscriptionId}' not found.");

        subscription.Status = status;
        subscription.LastModifiedAt = DateTime.UtcNow;

        _subscriptionRepository.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static SubscriptionPlanDto MapPlanToDto(SubscriptionPlan p) =>
        new(p.Id, p.Name, p.Description, p.Price, p.BillingCycle, p.DurationDays, p.Features, p.IsActive);

    private static SubscriptionDto MapSubscriptionToDto(Subscription s) =>
        new(
            s.Id,
            s.UserId,
            s.User?.FullName ?? string.Empty,
            s.User?.Email ?? string.Empty,
            s.PlanId,
            s.Plan?.Name ?? string.Empty,
            s.Plan?.Price ?? 0m,
            s.StartDate,
            s.EndDate,
            s.AutoRenew,
            s.Status
        );
}
