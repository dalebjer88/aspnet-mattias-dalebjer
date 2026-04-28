using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Features.Account;

public class AccountService : IAccountService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityUserService _identityUserService;

    public AccountService(
        IUserProfileRepository userProfileRepository,
        ICurrentUserService currentUserService,
        IIdentityUserService identityUserService)
    {
        _userProfileRepository = userProfileRepository;
        _currentUserService = currentUserService;
        _identityUserService = identityUserService;
    }

    public async Task<AboutMeDto?> GetAboutMeAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId) || string.IsNullOrWhiteSpace(_currentUserService.Email))
        {
            return null;
        }

        var userProfile = await _userProfileRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);

        return new AboutMeDto
        {
            Email = _currentUserService.Email,
            FirstName = userProfile?.FirstName,
            LastName = userProfile?.LastName,
            PhoneNumber = userProfile?.PhoneNumber,
            ProfileImagePath = userProfile?.ProfileImagePath
        };
    }

    public async Task SaveAboutMeAsync(UpdateAboutMeRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            throw new InvalidOperationException("Current user was not found.");
        }

        var userProfile = await _userProfileRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);

        if (userProfile is null)
        {
            userProfile = new UserProfile
            {
                UserId = _currentUserService.UserId
            };

            await _userProfileRepository.AddAsync(userProfile, cancellationToken);
        }

        userProfile.FirstName = Normalize(request.FirstName);
        userProfile.LastName = Normalize(request.LastName);
        userProfile.PhoneNumber = Normalize(request.PhoneNumber);

        if (!string.IsNullOrWhiteSpace(request.ProfileImagePath))
        {
            userProfile.ProfileImagePath = request.ProfileImagePath;
        }

        await _userProfileRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAccountAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return false;
        }

        return await _identityUserService.DeleteCurrentUserAsync(cancellationToken);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}