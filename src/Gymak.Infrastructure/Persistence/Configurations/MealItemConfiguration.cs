using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class MealItemConfiguration : IEntityTypeConfiguration<MealItem>
{
    public void Configure(EntityTypeBuilder<MealItem> builder)
    {
        builder.HasKey(mi => mi.MealItemId);

        builder.Property(mi => mi.MealType)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(mi => mi.QuantityInGrams)
            .HasPrecision(6, 2)
            .IsRequired();

        builder.HasOne(mi => mi.DietPlan)
            .WithMany(dp => dp.MealItems)
            .HasForeignKey(mi => mi.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mi => mi.FoodItem)
            .WithMany(f => f.MealItems)
            .HasForeignKey(mi => mi.FoodItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
