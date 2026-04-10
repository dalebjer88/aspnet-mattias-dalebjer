using CoreFitnessClub.Application.Abstractions;

namespace CoreFitnessClub.Application.Features.Memberships;

public class ReadMembershipService : IReadMembershipService
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly ICurrentUserService _currentUserService;

    public ReadMembershipService(
        IMembershipRepository membershipRepository,
        ICurrentUserService currentUserService)
    {
        _membershipRepository = membershipRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<MembershipPlanDto>> GetPlansAsync(CancellationToken cancellationToken = default)
    {
        var plans = await _membershipRepository.GetPlansAsync(cancellationToken);

        return plans.Select(x => new MembershipPlanDto
        {
            Id = x.Id,
            Name = x.Name,
            PricePerMonth = x.PricePerMonth,
            ClassesPerMonth = x.ClassesPerMonth,
            TrialWeeks = x.TrialWeeks,
            Features = x.Features.Select(feature => feature.Text).ToList()
        }).ToList();
    }

    public async Task<MyMembershipDto?> GetMyMembershipAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return null;
        }

        var membership = await _membershipRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);

        if (membership is null)
        {
            return null;
        }

        var plans = await _membershipRepository.GetPlansAsync(cancellationToken);
        var plan = plans.FirstOrDefault(x => x.Id == membership.MembershipPlanId);

        if (plan is null)
        {
            return null;
        }

        return new MyMembershipDto
        {
            PlanName = plan.Name,
            PricePerMonth = plan.PricePerMonth,
            ClassesPerMonth = plan.ClassesPerMonth,
            TrialWeeks = plan.TrialWeeks,
            Features = plan.Features.Select(feature => feature.Text).ToList(),
            Status = membership.Status.ToString(),
            StartDate = membership.StartDate
        };
    }
}