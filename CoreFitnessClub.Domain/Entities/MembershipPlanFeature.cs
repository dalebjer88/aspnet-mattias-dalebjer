namespace CoreFitnessClub.Domain.Entities;

public class MembershipPlanFeature
{
    public int Id { get; set; }
    public int MembershipPlanId { get; set; }
    public string Text { get; set; } = string.Empty;
}