using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class TrainerClientConfiguration : IEntityTypeConfiguration<TrainerClient>
{
    public void Configure(EntityTypeBuilder<TrainerClient> builder)
    {
        builder.HasKey(tc => tc.AssignmentId);

        builder.Property(tc => tc.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(tc => tc.Trainer)
            .WithMany(u => u.TrainerAssignments)
            .HasForeignKey(tc => tc.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tc => tc.Client)
            .WithMany(u => u.ClientAssignments)
            .HasForeignKey(tc => tc.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
