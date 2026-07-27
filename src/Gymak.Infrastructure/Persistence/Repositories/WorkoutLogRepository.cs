using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class WorkoutLogRepository : IWorkoutLogRepository
{
    private readonly AppDbContext _context;

    public WorkoutLogRepository(AppDbContext context) => _context = context;

    public async Task<WorkoutLog?> GetByIdAsync(Guid logId, CancellationToken cancellationToken = default)
        => await _context.WorkoutLogs
            .Include(wl => wl.Exercise)
            .FirstOrDefaultAsync(wl => wl.LogId == logId, cancellationToken);

    public async Task<IReadOnlyList<WorkoutLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.WorkoutLogs
            .AsNoTracking()
            .Include(wl => wl.Exercise)
            .Where(wl => wl.UserId == userId)
            .OrderByDescending(wl => wl.LogDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<WorkoutLog>> GetByUserAndDateRangeAsync(
        Guid userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => await _context.WorkoutLogs
            .AsNoTracking()
            .Include(wl => wl.Exercise)
            .Where(wl => wl.UserId == userId && wl.LogDate >= startDate && wl.LogDate <= endDate)
            .OrderByDescending(wl => wl.LogDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(WorkoutLog log, CancellationToken cancellationToken = default)
        => await _context.WorkoutLogs.AddAsync(log, cancellationToken);

    public void Delete(WorkoutLog log) => _context.WorkoutLogs.Remove(log);
}
