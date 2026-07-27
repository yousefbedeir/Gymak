using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class MuscleGroupRepository : IMuscleGroupRepository
{
    private readonly AppDbContext _context;

    public MuscleGroupRepository(AppDbContext context) => _context = context;

    public async Task<MuscleGroup?> GetByIdAsync(Guid muscleGroupId, CancellationToken cancellationToken = default)
        => await _context.MuscleGroups
            .Include(mg => mg.Exercises)
            .FirstOrDefaultAsync(mg => mg.MuscleGroupId == muscleGroupId, cancellationToken);

    public async Task<IReadOnlyList<MuscleGroup>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.MuscleGroups
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task AddAsync(MuscleGroup muscleGroup, CancellationToken cancellationToken = default)
        => await _context.MuscleGroups.AddAsync(muscleGroup, cancellationToken);

    public void Update(MuscleGroup muscleGroup) => _context.MuscleGroups.Update(muscleGroup);

    public void Delete(MuscleGroup muscleGroup) => _context.MuscleGroups.Remove(muscleGroup);
}
