using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record SubscriptionDto(
    Guid SubscriptionId,
    Guid UserId,
    string UserName,
    string UserEmail,
    Guid PlanId,
    string PlanName,
    decimal Price,
    DateTime StartDate,
    DateTime EndDate,
    bool AutoRenew,
    SubscriptionStatus Status
);

public record CreateSubscriptionRequest(
    Guid UserId,
    Guid PlanId,
    DateTime StartDate,
    bool AutoRenew
);

public record UpdateSubscriptionStatusRequest(
    Guid SubscriptionId,
    SubscriptionStatus Status
);
