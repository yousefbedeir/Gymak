namespace Gymak.Domain.Entities;

public class WorkoutLog
{
    public Guid LogId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ExerciseId { get; set; }
    public DateTime LogDate { get; set; } = DateTime.UtcNow;
    public int SetsCompleted { get; set; }
    public int RepsCompleted { get; set; }
    public decimal WeightUsed { get; set; }
    public int CaloriesBurned { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}
