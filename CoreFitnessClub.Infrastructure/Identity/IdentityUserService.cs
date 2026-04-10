using CoreFitnessClub.Application.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace CoreFitnessClub.Infrastructure.Identity;

public class IdentityUserService : IIdentityUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public IdentityUserService(
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
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

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }
}