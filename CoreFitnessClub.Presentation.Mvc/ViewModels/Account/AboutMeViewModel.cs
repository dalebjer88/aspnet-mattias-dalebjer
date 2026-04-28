using System.ComponentModel.DataAnnotations;
namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Account;

public class AboutMeViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    [RegularExpression(@"^[A-Za-zÅÄÖåäö\s\-']*$",
        ErrorMessage = "First name can only contain letters, spaces, hyphens and apostrophes.")]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    [RegularExpression(@"^[A-Za-zÅÄÖåäö\s\-']*$",
        ErrorMessage = "Last name can only contain letters, spaces, hyphens and apostrophes.")]
    public string? LastName { get; set; }

    [Phone]
    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    public string? ProfileImagePath { get; set; }

    public IFormFile? ProfileImage { get; set; }
}