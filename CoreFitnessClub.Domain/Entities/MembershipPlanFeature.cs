namespace CoreFitnessClub.Domain.Entities;

public class MembershipPlanFeature : BaseEntity
{
    public int MembershipPlanId { get; set; }
    public string Text { get; set; } = string.Empty;
}