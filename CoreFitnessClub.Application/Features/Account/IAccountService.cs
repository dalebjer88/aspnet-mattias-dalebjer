namespace CoreFitnessClub.Application.Features.Account;

public interface IAccountService
{
    Task<AboutMeDto?> GetAboutMeAsync(CancellationToken cancellationToken = default);
    Task SaveAboutMeAsync(UpdateAboutMeRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAccountAsync(CancellationToken cancellationToken = default);
}