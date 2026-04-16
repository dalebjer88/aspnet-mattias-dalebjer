using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Auth;

public class VerifySignUpEmailViewModel
{
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Code is required.")]
    public string Code { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}