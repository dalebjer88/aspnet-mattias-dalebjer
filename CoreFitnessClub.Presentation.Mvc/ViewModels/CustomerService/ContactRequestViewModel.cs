using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.CustomerService;

public class ContactRequestViewModel
{
[Required(ErrorMessage = "First name is required.")]
[StringLength(100, ErrorMessage = "Please enter a real name.")]
[RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$", ErrorMessage = "Please enter a real name.")]
public string FirstName { get; set; } = string.Empty;

[Required(ErrorMessage = "Last name is required.")]
[StringLength(100, ErrorMessage = "Please enter a real name.")]
[RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$", ErrorMessage = "Please enter a real name.")]
public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "Phone number cannot be longer than 30 characters.")]
    [RegularExpression(@"^[0-9+\-\s()]*$", ErrorMessage = "Please enter a real number.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    [StringLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters.")]
    public string Message { get; set; } = string.Empty;
}