namespace CoreFitnessClub.Application.Abstractions;

public interface IIdentityUserService
{
    Task<bool> DeleteCurrentUserAsync(CancellationToken cancellationToken = default);
}