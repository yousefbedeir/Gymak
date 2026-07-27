using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gymak.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Module 0
    DbSet<Member> Members { get; }

    // Module 1: User & Identity
    DbSet<User> Users { get; }
    DbSet<MemberProfile> MemberProfiles { get; }
    DbSet<TrainerClient> TrainerClients { get; }

    // Module 2: Exercises & Workouts
    DbSet<MuscleGroup> MuscleGroups { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<WorkoutPlan> WorkoutPlans { get; }
    DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; }
    DbSet<WorkoutLog> WorkoutLogs { get; }

    // Module 3: Nutrition & Diet
    DbSet<FoodItem> FoodItems { get; }
    DbSet<DietPlan> DietPlans { get; }
    DbSet<MealItem> MealItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
