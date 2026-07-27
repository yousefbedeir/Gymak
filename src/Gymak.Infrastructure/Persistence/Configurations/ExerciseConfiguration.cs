using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.ExerciseId);

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.ExerciseType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(e => e.MediaUrl)
            .HasMaxLength(500);

        builder.HasOne(e => e.MuscleGroup)
            .WithMany(mg => mg.Exercises)
            .HasForeignKey(e => e.MuscleGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
