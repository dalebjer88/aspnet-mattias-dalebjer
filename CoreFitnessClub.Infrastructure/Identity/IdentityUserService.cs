using CoreFitnessClub.Application.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace CoreFitnessClub.Infrastructure.Identity;

public class IdentityUserService : IIdentityUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBookingRepository _bookingRepository;

    public IdentityUserService(
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService,
        IBookingRepository bookingRepository)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _bookingRepository = bookingRepository;
    }

    public async Task<bool> DeleteCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

        if (user is null)
        {
            return true;
        }

        await _bookingRepository.RemoveByUserIdAsync(user.Id, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        var logins = await _userManager.GetLoginsAsync(user);

        foreach (var login in logins)
        {
            var removeLoginResult = await _userManager.RemoveLoginAsync(
                user,
                login.LoginProvider,
                login.ProviderKey);

            if (!removeLoginResult.Succeeded)
            {
                return false;
            }
        }

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }
}