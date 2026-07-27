namespace Gymak.Domain.Entities;

public class WorkoutPlanExercise
{
    public Guid PlanExerciseId { get; set; } = Guid.NewGuid();
    public Guid WorkoutPlanId { get; set; }
    public Guid ExerciseId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int RestTimeSeconds { get; set; }

    // Navigation Properties
    public WorkoutPlan WorkoutPlan { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
}
