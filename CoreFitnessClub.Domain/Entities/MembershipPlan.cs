namespace CoreFitnessClub.Domain.Entities;

public class MembershipPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; }
    public int ClassesPerMonth { get; set; }
    public int TrialWeeks { get; set; }
    public ICollection<MembershipPlanFeature> Features { get; set; } = [];
    public byte[]? RowVersion { get; set; }
}