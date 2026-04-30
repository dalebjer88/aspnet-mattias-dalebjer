using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Common.Exceptions;
using Microsoft.Data.SqlClient;
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

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            throw new DuplicateEntityException("You already have a membership.", exception);
        }
    }
    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException &&
               sqlException.Number is 2601 or 2627;
    }
}