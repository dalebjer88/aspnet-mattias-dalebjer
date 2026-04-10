using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Infrastructure.Data.Repositories;

public class MembershipRepository : IMembershipRepository
{
    private readonly CoreFitnessClubDbContext _dbContext;

    public MembershipRepository(CoreFitnessClubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MembershipPlan>> GetPlansAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.MembershipPlans
            .Include(x => x.Features)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Membership?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Memberships
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(Membership membership, CancellationToken cancellationToken = default)
    {
        await _dbContext.Memberships.AddAsync(membership, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}