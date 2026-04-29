using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Memberships;

public class CreateMembershipViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid membership plan.")]
    public int MembershipPlanId { get; set; }
}