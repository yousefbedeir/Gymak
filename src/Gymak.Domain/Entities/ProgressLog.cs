namespace Gymak.Domain.Entities;

public class ProgressLog : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime LogDate { get; set; } = DateTime.UtcNow;
    
    // Core Metrics
    public decimal Weight { get; set; } // in kg
    public decimal Height { get; set; } // in cm
    public decimal? BodyFatPercentage { get; set; }
    public decimal? MuscleMassPercentage { get; set; }

    // Circumferences (in cm)
    public decimal? ChestCm { get; set; }
    public decimal? WaistCm { get; set; }
    public decimal? HipsCm { get; set; }
    public decimal? BicepsCm { get; set; }
    public decimal? ThighsCm { get; set; }

    public string? Notes { get; set; }

    // Navigation Properties
    public User User { get; set; } = null!;

    // Calculated Property
    public decimal BMI
    {
        get
        {
            if (Height <= 0) return 0;
            var heightInMeters = Height / 100m;
            return Math.Round(Weight / (heightInMeters * heightInMeters), 2);
        }
    }
}
