using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public PaymentStatus Status { get; set; } = PaymentStatus.Completed;
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }

    // Navigation Properties
    public Subscription Subscription { get; set; } = null!;
    public User User { get; set; } = null!;
}
