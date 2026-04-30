namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Memberships;

public class MembershipCardViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerMonth { get; set; }
    public int ClassesPerMonth { get; set; }
    public int TrialWeeks { get; set; }
    public List<string> Features { get; set; } = [];
    public string? ReturnUrl { get; set; }
}