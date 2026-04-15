using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Auth;

public class SignInViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

    public List<string> ExternalProviders { get; set; } = [];
}