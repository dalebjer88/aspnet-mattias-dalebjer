using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Abstractions;

public interface IMembershipRepository
{
    Task<List<MembershipPlan>> GetPlansAsync(CancellationToken cancellationToken = default);
    Task<Membership?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(Membership membership, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}