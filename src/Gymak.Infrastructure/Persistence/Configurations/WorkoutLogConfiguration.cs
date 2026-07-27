using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class WorkoutLogConfiguration : IEntityTypeConfiguration<WorkoutLog>
{
    public void Configure(EntityTypeBuilder<WorkoutLog> builder)
    {
        builder.HasKey(wl => wl.LogId);

        builder.Property(wl => wl.WeightUsed)
            .HasPrecision(5, 2);

        builder.HasOne(wl => wl.User)
            .WithMany()
            .HasForeignKey(wl => wl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wl => wl.Exercise)
            .WithMany(e => e.WorkoutLogs)
            .HasForeignKey(wl => wl.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
