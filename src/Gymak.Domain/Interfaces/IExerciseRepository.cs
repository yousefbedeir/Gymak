using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(Guid exerciseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Exercise>> GetByMuscleGroupIdAsync(Guid muscleGroupId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Exercise>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default);
    void Update(Exercise exercise);
    void Delete(Exercise exercise);
}
