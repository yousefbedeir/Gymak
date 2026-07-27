using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IMuscleGroupRepository
{
    Task<MuscleGroup?> GetByIdAsync(Guid muscleGroupId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MuscleGroup>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(MuscleGroup muscleGroup, CancellationToken cancellationToken = default);
    void Update(MuscleGroup muscleGroup);
    void Delete(MuscleGroup muscleGroup);
}
