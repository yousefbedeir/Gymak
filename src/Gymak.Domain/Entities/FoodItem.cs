namespace Gymak.Domain.Entities;

public class FoodItem
{
    public Guid FoodItemId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal CaloriesPer100g { get; set; }
    public decimal ProteinPer100g { get; set; }
    public decimal CarbsPer100g { get; set; }
    public decimal FatPer100g { get; set; }
    public decimal FiberPer100g { get; set; }

    // Navigation Property
    public ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();
}
