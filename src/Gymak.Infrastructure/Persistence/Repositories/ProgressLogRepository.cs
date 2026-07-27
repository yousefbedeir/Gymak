using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class ProgressLogRepository : IProgressLogRepository
{
    private readonly AppDbContext _context;

    public ProgressLogRepository(AppDbContext context) => _context = context;

    public async Task<ProgressLog?> GetByIdAsync(Guid logId, CancellationToken cancellationToken = default)
        => await _context.ProgressLogs
            .Include(pl => pl.User)
            .FirstOrDefaultAsync(pl => pl.Id == logId, cancellationToken);

    public async Task<IReadOnlyList<ProgressLog>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.ProgressLogs
            .AsNoTracking()
            .Where(pl => pl.UserId == userId)
            .OrderByDescending(pl => pl.LogDate)
            .ToListAsync(cancellationToken);

    public async Task<ProgressLog?> GetLatestByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.ProgressLogs
            .AsNoTracking()
            .Where(pl => pl.UserId == userId)
            .OrderByDescending(pl => pl.LogDate)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task AddAsync(ProgressLog log, CancellationToken cancellationToken = default)
        => await _context.ProgressLogs.AddAsync(log, cancellationToken);

    public void Update(ProgressLog log) => _context.ProgressLogs.Update(log);

    public void Delete(ProgressLog log) => _context.ProgressLogs.Remove(log);
}
