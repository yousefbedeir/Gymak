namespace Gymak.Domain.Entities;

public class WorkoutPlan
{
    public Guid WorkoutPlanId { get; set; } = Guid.NewGuid();
    public Guid TrainerId { get; set; }
    public Guid MemberId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation Properties
    public User Trainer { get; set; } = null!;
    public User Member { get; set; } = null!;
    public ICollection<WorkoutPlanExercise> PlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
