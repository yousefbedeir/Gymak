using Gymak.Domain.Enums;

namespace Gymak.Domain.Entities;

public class Exercise
{
    public Guid ExerciseId { get; set; } = Guid.NewGuid();
    public Guid MuscleGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ExerciseType ExerciseType { get; set; } = ExerciseType.Strength;
    public string? MediaUrl { get; set; }

    // Navigation Properties
    public MuscleGroup MuscleGroup { get; set; } = null!;
    public ICollection<WorkoutPlanExercise> PlanExercises { get; set; } = new List<WorkoutPlanExercise>();
    public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
}
