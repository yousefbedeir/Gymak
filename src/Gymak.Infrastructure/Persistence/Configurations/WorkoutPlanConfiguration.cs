using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.HasKey(wp => wp.WorkoutPlanId);

        builder.Property(wp => wp.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasOne(wp => wp.Trainer)
            .WithMany()
            .HasForeignKey(wp => wp.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(wp => wp.Member)
            .WithMany()
            .HasForeignKey(wp => wp.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
