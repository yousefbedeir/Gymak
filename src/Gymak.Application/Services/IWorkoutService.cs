using Gymak.Application.DTOs;

namespace Gymak.Application.Services;

public interface IWorkoutService
{
    // Muscle Groups
    Task<IReadOnlyList<MuscleGroupDto>> GetMuscleGroupsAsync(CancellationToken cancellationToken = default);
    Task<Guid> CreateMuscleGroupAsync(CreateMuscleGroupRequest request, CancellationToken cancellationToken = default);

    // Exercises
    Task<IReadOnlyList<ExerciseDto>> GetAllExercisesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ExerciseDto>> GetExercisesByMuscleGroupAsync(Guid muscleGroupId, CancellationToken cancellationToken = default);
    Task<Guid> CreateExerciseAsync(CreateExerciseRequest request, CancellationToken cancellationToken = default);

    // Workout Plans
    Task<WorkoutPlanDto?> GetWorkoutPlanByIdAsync(Guid planId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkoutPlanDto>> GetWorkoutPlansByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<Guid> CreateWorkoutPlanAsync(CreateWorkoutPlanRequest request, CancellationToken cancellationToken = default);

    // Workout Logs
    Task<IReadOnlyList<WorkoutLogDto>> GetUserWorkoutLogsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> CreateWorkoutLogAsync(CreateWorkoutLogRequest request, CancellationToken cancellationToken = default);
}
