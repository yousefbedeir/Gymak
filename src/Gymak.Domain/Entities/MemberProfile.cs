namespace Gymak.Domain.Entities;

public class MemberProfile
{
    public Guid ProfileId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public decimal Height { get; set; }
    public decimal CurrentWeight { get; set; }
    public string? FitnessGoal { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;
}
