namespace CoreFitnessClub.Application.Features.Account;

public class AboutMeDto
{
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImagePath { get; set; }
}