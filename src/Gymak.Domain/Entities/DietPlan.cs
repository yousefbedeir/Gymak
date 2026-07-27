namespace Gymak.Domain.Entities;

public class DietPlan
{
    public Guid DietPlanId { get; set; } = Guid.NewGuid();
    public Guid TrainerId { get; set; }
    public Guid MemberId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DailyCalorieGoal { get; set; }
    public decimal DailyProteinGoal { get; set; }
    public decimal DailyCarbsGoal { get; set; }
    public decimal DailyFatGoal { get; set; }

    // Navigation Properties
    public User Trainer { get; set; } = null!;
    public User Member { get; set; } = null!;
    public ICollection<MealItem> MealItems { get; set; } = new List<MealItem>();
}
