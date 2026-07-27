using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public BillingCycle BillingCycle { get; set; } = BillingCycle.Monthly;
    public int DurationDays { get; set; } = 30;
    public string? Features { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
