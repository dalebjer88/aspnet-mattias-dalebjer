using System.Reflection;
using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Features.Memberships;
using CoreFitnessClub.Domain.Entities;
using NSubstitute;

namespace CoreFitnessClub.Tests.Unit.Application;

public class MembershipServiceTests
{
    [Fact]
    public async Task CreateMembershipAsync_WhenUserIsMissing_ReturnsFailure()
    {
        var repository = Substitute.For<IMembershipRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();

        currentUserService.UserId.Returns((string?)null);

        var service = new MembershipService(repository, currentUserService);

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = 1
        };

        var result = await service.CreateMembershipAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("You must be logged in to create a membership.", result.ErrorMessage);

        await repository.DidNotReceive().GetPlansAsync(Arg.Any<CancellationToken>());
        await repository.DidNotReceive().AddAsync(
            Arg.Any<Membership>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateMembershipAsync_WhenPlanDoesNotExist_ReturnsFailure()
    {
        var repository = Substitute.For<IMembershipRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();

        currentUserService.UserId.Returns("user-1");

        repository.GetPlansAsync(Arg.Any<CancellationToken>())
            .Returns([]);

        var service = new MembershipService(repository, currentUserService);

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = 1
        };

        var result = await service.CreateMembershipAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("The selected membership plan does not exist.", result.ErrorMessage);

        await repository.DidNotReceive().AddAsync(
            Arg.Any<Membership>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateMembershipAsync_WhenUserAlreadyHasMembership_ReturnsFailure()
    {
        var repository = Substitute.For<IMembershipRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();

        currentUserService.UserId.Returns("user-1");

        var plan = CreateMembershipPlan(1);

        repository.GetPlansAsync(Arg.Any<CancellationToken>())
            .Returns([plan]);

        repository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(new Membership
            {
                UserId = "user-1",
                MembershipPlanId = 1,
                Status = MembershipStatus.Active,
                StartDate = DateTime.UtcNow
            });

        var service = new MembershipService(repository, currentUserService);

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = 1
        };

        var result = await service.CreateMembershipAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("You already have a membership.", result.ErrorMessage);

        await repository.DidNotReceive().AddAsync(
            Arg.Any<Membership>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateMembershipAsync_WithValidRequest_ReturnsSuccess()
    {
        var repository = Substitute.For<IMembershipRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();

        currentUserService.UserId.Returns("user-1");

        var plan = CreateMembershipPlan(1);

        repository.GetPlansAsync(Arg.Any<CancellationToken>())
            .Returns([plan]);

        repository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns((Membership?)null);

        var service = new MembershipService(repository, currentUserService);

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = 1
        };

        var result = await service.CreateMembershipAsync(request);

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);

        await repository.Received(1).AddAsync(
            Arg.Is<Membership>(membership =>
                membership.UserId == "user-1" &&
                membership.MembershipPlanId == 1 &&
                membership.Status == MembershipStatus.Active),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static MembershipPlan CreateMembershipPlan(int id)
    {
        var plan = new MembershipPlan
        {
            Name = "Basic",
            PricePerMonth = 299,
            ClassesPerMonth = 8,
            TrialWeeks = 2
        };

        SetEntityId(plan, id);

        return plan;
    }

    private static void SetEntityId(BaseEntity entity, int id)
    {
        var property = typeof(BaseEntity).GetProperty(
            nameof(BaseEntity.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        property!.SetValue(entity, id);
    }
}