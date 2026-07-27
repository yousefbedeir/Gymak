using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record SubscriptionPlanDto(
    Guid PlanId,
    string Name,
    string? Description,
    decimal Price,
    BillingCycle BillingCycle,
    int DurationDays,
    string? Features,
    bool IsActive
);

public record CreateSubscriptionPlanRequest(
    string Name,
    string? Description,
    decimal Price,
    BillingCycle BillingCycle,
    int DurationDays,
    string? Features
);

public record UpdateSubscriptionPlanRequest(
    Guid PlanId,
    string Name,
    string? Description,
    decimal Price,
    BillingCycle BillingCycle,
    int DurationDays,
    string? Features,
    bool IsActive
);
