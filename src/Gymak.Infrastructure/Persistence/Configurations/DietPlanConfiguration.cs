using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class DietPlanConfiguration : IEntityTypeConfiguration<DietPlan>
{
    public void Configure(EntityTypeBuilder<DietPlan> builder)
    {
        builder.HasKey(dp => dp.DietPlanId);

        builder.Property(dp => dp.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(dp => dp.DailyProteinGoal).HasPrecision(5, 2);
        builder.Property(dp => dp.DailyCarbsGoal).HasPrecision(5, 2);
        builder.Property(dp => dp.DailyFatGoal).HasPrecision(5, 2);

        builder.HasOne(dp => dp.Trainer)
            .WithMany()
            .HasForeignKey(dp => dp.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.Member)
            .WithMany()
            .HasForeignKey(dp => dp.MemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
