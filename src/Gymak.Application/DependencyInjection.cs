using FluentValidation;
using Gymak.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gymak.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<INutritionService, NutritionService>();

        return services;
    }
}
