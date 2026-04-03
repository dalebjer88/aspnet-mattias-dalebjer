namespace CoreFitnessClub.Application.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
}