using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class ProgressLogConfiguration : IEntityTypeConfiguration<ProgressLog>
{
    public void Configure(EntityTypeBuilder<ProgressLog> builder)
    {
        builder.HasKey(pl => pl.Id);

        builder.Property(pl => pl.Weight)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(pl => pl.Height)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(pl => pl.BodyFatPercentage).HasPrecision(4, 2);
        builder.Property(pl => pl.MuscleMassPercentage).HasPrecision(4, 2);
        builder.Property(pl => pl.ChestCm).HasPrecision(5, 2);
        builder.Property(pl => pl.WaistCm).HasPrecision(5, 2);
        builder.Property(pl => pl.HipsCm).HasPrecision(5, 2);
        builder.Property(pl => pl.BicepsCm).HasPrecision(5, 2);
        builder.Property(pl => pl.ThighsCm).HasPrecision(5, 2);

        builder.Property(pl => pl.Notes)
            .HasMaxLength(500);

        builder.HasOne(pl => pl.User)
            .WithMany()
            .HasForeignKey(pl => pl.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
