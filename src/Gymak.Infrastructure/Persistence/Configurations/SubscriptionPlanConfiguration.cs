using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sp => sp.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(sp => sp.Description)
            .HasMaxLength(500);

        builder.Property(sp => sp.Features)
            .HasMaxLength(2000);
    }
}
