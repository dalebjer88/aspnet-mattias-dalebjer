namespace CoreFitnessClub.Application.Features.Memberships;

public class MyMembershipDto
{
    public string PlanName { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; }
    public int ClassesPerMonth { get; set; }
    public int TrialWeeks { get; set; }
    public List<string> Features { get; set; } = [];
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
}