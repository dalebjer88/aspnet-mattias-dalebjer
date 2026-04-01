using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Auth;

public class SignUpViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}