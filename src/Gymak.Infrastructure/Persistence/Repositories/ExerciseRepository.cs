using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly AppDbContext _context;

    public ExerciseRepository(AppDbContext context) => _context = context;

    public async Task<Exercise?> GetByIdAsync(Guid exerciseId, CancellationToken cancellationToken = default)
        => await _context.Exercises
            .Include(e => e.MuscleGroup)
            .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId, cancellationToken);

    public async Task<IReadOnlyList<Exercise>> GetByMuscleGroupIdAsync(Guid muscleGroupId, CancellationToken cancellationToken = default)
        => await _context.Exercises
            .AsNoTracking()
            .Include(e => e.MuscleGroup)
            .Where(e => e.MuscleGroupId == muscleGroupId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Exercise>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Exercises
            .AsNoTracking()
            .Include(e => e.MuscleGroup)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Exercise exercise, CancellationToken cancellationToken = default)
        => await _context.Exercises.AddAsync(exercise, cancellationToken);

    public void Update(Exercise exercise) => _context.Exercises.Update(exercise);

    public void Delete(Exercise exercise) => _context.Exercises.Remove(exercise);
}
