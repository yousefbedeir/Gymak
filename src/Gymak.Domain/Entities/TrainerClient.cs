using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class TrainerClient
{
    public Guid AssignmentId { get; set; } = Guid.NewGuid();
    public Guid TrainerId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Active;

    // Navigation Properties
    public User Trainer { get; set; } = null!;
    public User Client { get; set; } = null!;
}
