using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public bool AutoRenew { get; set; } = false;
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

    // Navigation Properties
    public User User { get; set; } = null!;
    public SubscriptionPlan Plan { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
