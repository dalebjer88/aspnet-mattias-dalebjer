using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoreFitnessClub.Infrastructure.Identity;

public class IdentityUserService : IIdentityUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IBookingRepository _bookingRepository;
    private readonly CoreFitnessClubDbContext _dbContext;

    public IdentityUserService(
        UserManager<AppUser> userManager,
        ICurrentUserService currentUserService,
        IBookingRepository bookingRepository,
        CoreFitnessClubDbContext dbContext)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _bookingRepository = bookingRepository;
        _dbContext = dbContext;
    }

    public async Task<bool> DeleteCurrentUserAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return false;
        }

        if (!_dbContext.Database.IsRelational())
        {
            return await DeleteCurrentUserDataAsync(cancellationToken);
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var deleted = await DeleteCurrentUserDataAsync(cancellationToken);

        if (!deleted)
        {
            await transaction.RollbackAsync(cancellationToken);
            return false;
        }

        await transaction.CommitAsync(cancellationToken);

        return true;
    }

    private async Task<bool> DeleteCurrentUserDataAsync(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);

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