namespace Gymak.Domain.Entities;

public class MuscleGroup
{
    public Guid MuscleGroupId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
