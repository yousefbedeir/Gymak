using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class TrainerClientRepository : ITrainerClientRepository
{
    private readonly AppDbContext _context;

    public TrainerClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TrainerClient?> GetByIdAsync(Guid assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.TrainerClients
            .Include(tc => tc.Trainer)
            .Include(tc => tc.Client)
            .FirstOrDefaultAsync(tc => tc.AssignmentId == assignmentId, cancellationToken);
    }

    public async Task<IReadOnlyList<TrainerClient>> GetClientsForTrainerAsync(Guid trainerId, CancellationToken cancellationToken = default)
    {
        return await _context.TrainerClients
            .AsNoTracking()
            .Include(tc => tc.Client)
            .Where(tc => tc.TrainerId == trainerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TrainerClient>> GetTrainersForClientAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.TrainerClients
            .AsNoTracking()
            .Include(tc => tc.Trainer)
            .Where(tc => tc.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid trainerId, Guid clientId, CancellationToken cancellationToken = default)
    {
        return await _context.TrainerClients
            .AnyAsync(tc => tc.TrainerId == trainerId && tc.ClientId == clientId, cancellationToken);
    }

    public async Task AddAsync(TrainerClient assignment, CancellationToken cancellationToken = default)
    {
        await _context.TrainerClients.AddAsync(assignment, cancellationToken);
    }

    public void Update(TrainerClient assignment)
    {
        _context.TrainerClients.Update(assignment);
    }
}
