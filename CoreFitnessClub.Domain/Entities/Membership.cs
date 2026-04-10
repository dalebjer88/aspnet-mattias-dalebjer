namespace CoreFitnessClub.Domain.Entities;

public class Membership
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int MembershipPlanId { get; set; }
    public MembershipStatus Status { get; set; }
    public DateTime StartDate { get; set; }
}