using CoreFitnessClub.Application.Common.Results;

namespace CoreFitnessClub.Application.Features.Memberships;

public interface IMembershipService
{
    Task<Result> CreateMembershipAsync(CreateMembershipRequest request, CancellationToken cancellationToken = default);
}