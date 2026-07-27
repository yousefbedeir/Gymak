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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMemberProfileRepository, MemberProfileRepository>();
        services.AddScoped<ITrainerClientRepository, TrainerClientRepository>();

        // Module 2: Exercises & Workouts
        services.AddScoped<IMuscleGroupRepository, MuscleGroupRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        services.AddScoped<IWorkoutLogRepository, WorkoutLogRepository>();

        return services;
    }
}
