using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Member> Members { get; }
    DbSet<User> Users { get; }
    DbSet<MemberProfile> MemberProfiles { get; }
    DbSet<TrainerClient> TrainerClients { get; }
    DbSet<MuscleGroup> MuscleGroups { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<WorkoutPlan> WorkoutPlans { get; }
    DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; }
    DbSet<WorkoutLog> WorkoutLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
