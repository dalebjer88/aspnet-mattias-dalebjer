using CoreFitnessClub.Application.Common.Results;
using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Domain.Entities;


namespace CoreFitnessClub.Application.Features.Memberships;

public class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly ICurrentUserService _currentUserService;

    public MembershipService(
        IMembershipRepository membershipRepository,
        ICurrentUserService currentUserService)
    {
        _membershipRepository = membershipRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result> CreateMembershipAsync(CreateMembershipRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
        {
            return Result.Failure("You must be logged in to create a membership.");
        }

        var plans = await _membershipRepository.GetPlansAsync(cancellationToken);
        var plan = plans.FirstOrDefault(x => x.Id == request.MembershipPlanId);

        if (plan is null)
        {
            return Result.Failure("The selected membership plan does not exist.");
        }

        var existingMembership = await _membershipRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);

        if (existingMembership is not null)
        {
            return Result.Failure("You already have a membership.");
        }

        var membership = new Membership
        {
            UserId = _currentUserService.UserId,
            MembershipPlanId = request.MembershipPlanId,
            Status = MembershipStatus.Active,
            StartDate = DateTime.UtcNow
        };

        await _membershipRepository.AddAsync(membership, cancellationToken);
        await _membershipRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}