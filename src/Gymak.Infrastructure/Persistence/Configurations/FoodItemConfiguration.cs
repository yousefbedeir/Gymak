using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.HasKey(f => f.FoodItemId);

        builder.Property(f => f.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.CaloriesPer100g).HasPrecision(7, 2);
        builder.Property(f => f.ProteinPer100g).HasPrecision(5, 2);
        builder.Property(f => f.CarbsPer100g).HasPrecision(5, 2);
        builder.Property(f => f.FatPer100g).HasPrecision(5, 2);
        builder.Property(f => f.FiberPer100g).HasPrecision(5, 2);
    }
}
