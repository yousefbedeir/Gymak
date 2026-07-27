using Gymak.Domain.Entities;

namespace Gymak.Domain.Interfaces;

public interface IProgressLogRepository
{
    Task<ProgressLog?> GetByIdAsync(Guid logId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProgressLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ProgressLog?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(ProgressLog log, CancellationToken cancellationToken = default);
    void Update(ProgressLog log);
    void Delete(ProgressLog log);
}
