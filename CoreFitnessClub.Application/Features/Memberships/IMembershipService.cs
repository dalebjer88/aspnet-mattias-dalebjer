namespace CoreFitnessClub.Application.Features.Memberships;

public interface IMembershipService
{
    Task<MembershipResult> CreateMembershipAsync(CreateMembershipRequest request, CancellationToken cancellationToken = default);
}