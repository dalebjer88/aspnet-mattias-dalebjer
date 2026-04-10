namespace CoreFitnessClub.Application.Features.Classes;

public class TrainingClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? InstructorName { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}