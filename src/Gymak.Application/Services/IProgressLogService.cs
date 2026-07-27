using Gymak.Application.DTOs;

namespace Gymak.Application.Services;

public interface IProgressLogService
{
    Task<ProgressLogDto?> GetByIdAsync(Guid logId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProgressLogDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ProgressLogDto?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Guid> LogProgressAsync(CreateProgressLogRequest request, CancellationToken cancellationToken = default);
    Task UpdateProgressLogAsync(UpdateProgressLogRequest request, CancellationToken cancellationToken = default);
    Task DeleteProgressLogAsync(Guid logId, CancellationToken cancellationToken = default);
}
