using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Infrastructure.Data.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly CoreFitnessClubDbContext _dbContext;

    public UserProfileRepository(CoreFitnessClubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default)
    {
        await _dbContext.UserProfiles.AddAsync(userProfile, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}