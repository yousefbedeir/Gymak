using Gymak.Domain.Entities;
using Gymak.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Infrastructure.Persistence.Repositories;

public class WorkoutPlanRepository : IWorkoutPlanRepository
{
    private readonly AppDbContext _context;

    public WorkoutPlanRepository(AppDbContext context) => _context = context;

    public async Task<WorkoutPlan?> GetByIdAsync(Guid workoutPlanId, CancellationToken cancellationToken = default)
        => await _context.WorkoutPlans
            .Include(wp => wp.Trainer)
            .Include(wp => wp.Member)
            .Include(wp => wp.PlanExercises)
                .ThenInclude(pe => pe.Exercise)
            .FirstOrDefaultAsync(wp => wp.WorkoutPlanId == workoutPlanId, cancellationToken);

    public async Task<IReadOnlyList<WorkoutPlan>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
        => await _context.WorkoutPlans
            .AsNoTracking()
            .Include(wp => wp.Trainer)
            .Include(wp => wp.PlanExercises)
                .ThenInclude(pe => pe.Exercise)
            .Where(wp => wp.MemberId == memberId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<WorkoutPlan>> GetByTrainerIdAsync(Guid trainerId, CancellationToken cancellationToken = default)
        => await _context.WorkoutPlans
            .AsNoTracking()
            .Include(wp => wp.Member)
            .Include(wp => wp.PlanExercises)
                .ThenInclude(pe => pe.Exercise)
            .Where(wp => wp.TrainerId == trainerId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(WorkoutPlan plan, CancellationToken cancellationToken = default)
        => await _context.WorkoutPlans.AddAsync(plan, cancellationToken);

    public void Update(WorkoutPlan plan) => _context.WorkoutPlans.Update(plan);

    public void Delete(WorkoutPlan plan) => _context.WorkoutPlans.Remove(plan);
}
