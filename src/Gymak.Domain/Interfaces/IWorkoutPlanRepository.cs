using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IWorkoutPlanRepository
{
    Task<WorkoutPlan?> GetByIdAsync(Guid workoutPlanId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkoutPlan>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkoutPlan>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default);
    Task AddAsync(WorkoutPlan plan, CancellationToken cancellationToken = default);
    void Update(WorkoutPlan plan);
    void Delete(WorkoutPlan plan);
}
