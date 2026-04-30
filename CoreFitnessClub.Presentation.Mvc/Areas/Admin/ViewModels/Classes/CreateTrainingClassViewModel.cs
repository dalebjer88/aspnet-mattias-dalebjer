using System.ComponentModel.DataAnnotations;

namespace CoreFitnessClub.Presentation.Mvc.Areas.Admin.ViewModels.Classes;

public class CreateTrainingClassViewModel : IValidatableObject
{
    [Display(Name = "Class name")]
    [Required(ErrorMessage = "Class name is required.")]
    [StringLength(100, ErrorMessage = "Class name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Category")]
    [Required(ErrorMessage = "Category is required.")]
    [StringLength(100, ErrorMessage = "Category cannot be longer than 100 characters.")]
    public string Category { get; set; } = string.Empty;

    [Display(Name = "Instructor name")]
    [Required(ErrorMessage = "Instructor name is required.")]
    [StringLength(100, ErrorMessage = "Instructor name cannot be longer than 100 characters.")]
    public string InstructorName { get; set; } = string.Empty;

    [Display(Name = "Class date")]
    [Required(ErrorMessage = "Class date is required.")]
    public DateOnly? Date { get; set; }

    [Display(Name = "Start time")]
    [Required(ErrorMessage = "Start time is required.")]
    public TimeOnly? StartTime { get; set; }

    [Display(Name = "End time")]
    [Required(ErrorMessage = "End time is required.")]
    public TimeOnly? EndTime { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);

        if (Date.HasValue && Date.Value < today)
        {
            yield return new ValidationResult(
                "Class date cannot be in the past.",
                new[] { nameof(Date) });
        }

        if (Date.HasValue && StartTime.HasValue && Date.Value == today && StartTime.Value <= currentTime)
        {
            yield return new ValidationResult(
                "Start time must be later than the current time.",
                new[] { nameof(StartTime) });
        }

        if (StartTime.HasValue && EndTime.HasValue && EndTime.Value <= StartTime.Value)
        {
            yield return new ValidationResult(
                "End time must be later than start time.",
                new[] { nameof(EndTime) });
        }
    }
}