using Gymak.Domain.Enums;

namespace Gymak.Application.DTOs;

public record MuscleGroupDto(Guid MuscleGroupId, string Name);
public record CreateMuscleGroupRequest(string Name);

public record ExerciseDto(
    Guid ExerciseId,
    Guid MuscleGroupId,
    string MuscleGroupName,
    string Name,
    string? Description,
    ExerciseType ExerciseType,
    string? MediaUrl
);

public record CreateExerciseRequest(
    Guid MuscleGroupId,
    string Name,
    string? Description,
    ExerciseType ExerciseType,
    string? MediaUrl
);

public record WorkoutPlanExerciseDto(
    Guid PlanExerciseId,
    Guid ExerciseId,
    string ExerciseName,
    DayOfWeek DayOfWeek,
    int Sets,
    int Reps,
    int RestTimeSeconds
);

public record WorkoutPlanDto(
    Guid WorkoutPlanId,
    Guid TrainerId,
    string TrainerName,
    Guid MemberId,
    string MemberName,
    string Title,
    string? Description,
    IReadOnlyList<WorkoutPlanExerciseDto> Exercises
);

public record CreateWorkoutPlanRequest(
    Guid TrainerId,
    Guid MemberId,
    string Title,
    string? Description,
    List<AddPlanExerciseRequest> Exercises
);

public record AddPlanExerciseRequest(
    Guid ExerciseId,
    DayOfWeek DayOfWeek,
    int Sets,
    int Reps,
    int RestTimeSeconds
);

public record WorkoutLogDto(
    Guid LogId,
    Guid UserId,
    Guid ExerciseId,
    string ExerciseName,
    DateTime LogDate,
    int SetsCompleted,
    int RepsCompleted,
    decimal WeightUsed,
    int CaloriesBurned
);

public record CreateWorkoutLogRequest(
    Guid UserId,
    Guid ExerciseId,
    DateTime LogDate,
    int SetsCompleted,
    int RepsCompleted,
    decimal WeightUsed,
    int CaloriesBurned
);
