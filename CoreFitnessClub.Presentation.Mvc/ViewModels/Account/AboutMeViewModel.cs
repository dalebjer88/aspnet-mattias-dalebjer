using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Account;

public class AboutMeViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Phone]
    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    public string? ProfileImagePath { get; set; }
}