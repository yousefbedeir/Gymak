using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class MemberProfileConfiguration : IEntityTypeConfiguration<MemberProfile>
{
    public void Configure(EntityTypeBuilder<MemberProfile> builder)
    {
        builder.HasKey(mp => mp.ProfileId);

        builder.Property(mp => mp.Gender)
            .HasMaxLength(20);

        builder.Property(mp => mp.Height)
            .HasPrecision(5, 2);

        builder.Property(mp => mp.CurrentWeight)
            .HasPrecision(5, 2);

        builder.Property(mp => mp.FitnessGoal)
            .HasMaxLength(500);
    }
}
