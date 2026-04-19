namespace CoreFitnessClub.Domain.Entities;

public class UserProfile : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImagePath { get; set; }
    public byte[]? RowVersion { get; set; }
}