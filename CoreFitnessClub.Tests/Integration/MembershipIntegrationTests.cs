using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Features.Memberships;
using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Infrastructure.Data;
using CoreFitnessClub.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace CoreFitnessClub.Tests.Integration;

public class MembershipIntegrationTests
{
    [Fact]
    public async Task CreateMembershipAsync_WithValidPlan_SavesMembershipInDatabase()
    {
        await using var dbContext = CreateDbContext();

        var plan = new MembershipPlan
        {
            Name = "Basic",
            PricePerMonth = 299,
            ClassesPerMonth = 8,
            TrialWeeks = 2
        };

        await dbContext.MembershipPlans.AddAsync(plan);
        await dbContext.SaveChangesAsync();

        var repository = new MembershipRepository(dbContext);
        var currentUserService = Substitute.For<ICurrentUserService>();

        currentUserService.UserId.Returns("user-1");

        var service = new MembershipService(repository, currentUserService);

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = plan.Id
        };

        var result = await service.CreateMembershipAsync(request);

        var savedMembership = await dbContext.Memberships.SingleAsync();

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
        Assert.Equal("user-1", savedMembership.UserId);
        Assert.Equal(plan.Id, savedMembership.MembershipPlanId);
        Assert.Equal(MembershipStatus.Active, savedMembership.Status);
        Assert.True(savedMembership.StartDate <= DateTime.UtcNow);
    }

    [Fact]
    public async Task CreateMembershipAsync_WhenUserAlreadyHasMembership_ReturnsFailure()
    {
        await using var dbContext = CreateDbContext();

        var plan = new MembershipPlan
        {
            Name = "Basic",
            PricePerMonth = 299,
            ClassesPerMonth = 8,
            TrialWeeks = 2
        };

        await dbContext.MembershipPlans.AddAsync(plan);
        await dbContext.SaveChangesAsync();

        var existingMembership = new Membership
        {
            UserId = "user-1",
            MembershipPlanId = plan.Id,
            Status = MembershipStatus.Active,
            StartDate = DateTime.UtcNow
        };

        await dbContext.Memberships.AddAsync(existingMembership);
        await dbContext.SaveChangesAsync();

        var repository = new MembershipRepository(dbContext);
        var currentUserService = Substitute.For<ICurrentUserService>();

        currentUserService.UserId.Returns("user-1");

        var service = new MembershipService(repository, currentUserService);

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = plan.Id
        };

        var result = await service.CreateMembershipAsync(request);

        var membershipCount = await dbContext.Memberships.CountAsync();

        Assert.False(result.Succeeded);
        Assert.Equal("You already have a membership.", result.ErrorMessage);
        Assert.Equal(1, membershipCount);
    }

    private static CoreFitnessClubDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CoreFitnessClubDbContext>()
            .UseInMemoryDatabase($"CoreFitnessClubTests-{Guid.NewGuid()}")
            .Options;

        return new CoreFitnessClubDbContext(options);
    }
}