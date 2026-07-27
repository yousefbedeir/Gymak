using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class WorkoutPlanExerciseConfiguration : IEntityTypeConfiguration<WorkoutPlanExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutPlanExercise> builder)
    {
        builder.HasKey(wpe => wpe.PlanExerciseId);

        builder.HasOne(wpe => wpe.WorkoutPlan)
            .WithMany(wp => wp.PlanExercises)
            .HasForeignKey(wpe => wpe.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wpe => wpe.Exercise)
            .WithMany(e => e.PlanExercises)
            .HasForeignKey(wpe => wpe.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
