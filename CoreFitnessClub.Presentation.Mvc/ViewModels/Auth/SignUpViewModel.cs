using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Auth;

public class SignUpViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions.")]
    public bool AcceptTerms { get; set; }

    public string? ReturnUrl { get; set; }

    public List<string> ExternalProviders { get; set; } = [];
}