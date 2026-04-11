namespace CoreFitnessClub.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int TrainingClassId { get; set; }
    public DateTime BookedAt { get; set; }
}