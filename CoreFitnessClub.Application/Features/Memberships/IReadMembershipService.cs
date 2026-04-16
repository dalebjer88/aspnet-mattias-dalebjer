namespace CoreFitnessClub.Application.Features.Memberships;

public interface IReadMembershipService
{
    Task<List<MembershipPlanDto>> GetPlansAsync(CancellationToken cancellationToken = default);
    Task<MyMembershipDto?> GetMyMembershipAsync(CancellationToken cancellationToken = default);
    Task<bool> HasActiveMembershipAsync(CancellationToken cancellationToken = default);
}