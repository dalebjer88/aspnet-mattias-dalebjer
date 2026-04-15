namespace CoreFitnessClub.Application.Features.Bookings;

public class UserBookingDto
{
    public int BookingId { get; set; }
    public int TrainingClassId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string InstructorName { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}