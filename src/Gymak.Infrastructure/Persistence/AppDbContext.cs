using Gymak.Application.Common.Interfaces;
using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Gymak.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Module 0 (Scaffold)
    public DbSet<Member> Members => Set<Member>();

    // Module 1: User & Identity
    public DbSet<User> Users => Set<User>();
    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();
    public DbSet<TrainerClient> TrainerClients => Set<TrainerClient>();

    // Module 2: Exercises & Workouts
    public DbSet<MuscleGroup> MuscleGroups => Set<MuscleGroup>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutPlanExercise> WorkoutPlanExercises => Set<WorkoutPlanExercise>();
    public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
