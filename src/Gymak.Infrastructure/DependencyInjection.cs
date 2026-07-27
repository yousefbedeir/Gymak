using Gymak.Application.Common.Interfaces;
using Gymak.Domain.Interfaces;
using Gymak.Infrastructure.Persistence;
using Gymak.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gymak.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        // Module 0
        services.AddScoped<IMemberRepository, MemberRepository>();

        // Module 1: User & Identity
        services.AddScoped<IPasswordHasher, Identity.PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMemberProfileRepository, MemberProfileRepository>();
        services.AddScoped<ITrainerClientRepository, TrainerClientRepository>();

        // Module 2: Exercises & Workouts
        services.AddScoped<IMuscleGroupRepository, MuscleGroupRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        services.AddScoped<IWorkoutLogRepository, WorkoutLogRepository>();

        // Module 3: Nutrition & Diet
        services.AddScoped<IFoodItemRepository, FoodItemRepository>();
        services.AddScoped<IDietPlanRepository, DietPlanRepository>();

        // Module 4: Subscriptions & Financials
        services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        // Module 5: Progress & Body Metrics
        services.AddScoped<IProgressLogRepository, ProgressLogRepository>();

        return services;
    }
}
