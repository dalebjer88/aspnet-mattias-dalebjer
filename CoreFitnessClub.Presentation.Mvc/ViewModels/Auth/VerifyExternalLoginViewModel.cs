using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Auth;

public class VerifyExternalLoginViewModel
{
    public string Email { get; set; } = null!;
    public string? ReturnUrl { get; set; }

    [Required(ErrorMessage = "Code is required.")]
    public string Code { get; set; } = null!;
}
