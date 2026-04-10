namespace CoreFitnessClub.Domain.Entities;

public class TrainingClass
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? InstructorName { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}