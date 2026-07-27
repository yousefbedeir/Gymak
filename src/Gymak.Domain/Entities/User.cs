using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Member;
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpiresAt { get; set; }

    // Navigation Properties
    public MemberProfile? Profile { get; set; }
    public ICollection<TrainerClient> TrainerAssignments { get; set; } = new List<TrainerClient>();
    public ICollection<TrainerClient> ClientAssignments { get; set; } = new List<TrainerClient>();
}
