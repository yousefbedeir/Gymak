using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IWorkoutLogRepository
{
    Task<WorkoutLog?> GetByIdAsync(Guid logId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkoutLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkoutLog>> GetByUserAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task AddAsync(WorkoutLog log, CancellationToken cancellationToken = default);
    void Delete(WorkoutLog log);
}
