using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Infrastructure.Data;
using CoreFitnessClub.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Tests.Integration;

public class BookingIntegrationTests
{
    [Fact]
    public async Task BookAsync_WithActiveMembershipAndFutureClass_SavesBookingInDatabase()
    {
        await using var dbContext = CreateDbContext();

        var trainingClass = await CreateFutureTrainingClassAsync(dbContext);
        await CreateActiveMembershipAsync(dbContext, "user-1");

        var service = CreateBookingService(dbContext);

        var result = await service.BookAsync("user-1", trainingClass.Id);

        var savedBooking = await dbContext.Bookings.SingleAsync();

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
        Assert.Equal("user-1", savedBooking.UserId);
        Assert.Equal(trainingClass.Id, savedBooking.TrainingClassId);
        Assert.True(savedBooking.BookedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task BookAsync_WhenSameClassIsBookedTwice_ReturnsFailure()
    {
        await using var dbContext = CreateDbContext();

        var trainingClass = await CreateFutureTrainingClassAsync(dbContext);
        await CreateActiveMembershipAsync(dbContext, "user-1");

        var service = CreateBookingService(dbContext);

        var firstResult = await service.BookAsync("user-1", trainingClass.Id);
        var secondResult = await service.BookAsync("user-1", trainingClass.Id);

        var bookingCount = await dbContext.Bookings.CountAsync();

        Assert.True(firstResult.Succeeded);
        Assert.False(secondResult.Succeeded);
        Assert.Equal("You have already booked this class.", secondResult.ErrorMessage);
        Assert.Equal(1, bookingCount);
    }

    [Fact]
    public async Task CancelAsync_WhenBookingExists_RemovesBookingFromDatabase()
    {
        await using var dbContext = CreateDbContext();

        var trainingClass = await CreateFutureTrainingClassAsync(dbContext);

        var booking = new Booking
        {
            UserId = "user-1",
            TrainingClassId = trainingClass.Id,
            BookedAt = DateTime.UtcNow
        };

        await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();

        var service = CreateBookingService(dbContext);

        var result = await service.CancelAsync("user-1", trainingClass.Id);

        var bookingExists = await dbContext.Bookings.AnyAsync();

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
        Assert.False(bookingExists);
    }

    [Fact]
    public async Task GetUserBookingsAsync_ReturnsUserBookingsWithClassDetails()
    {
        await using var dbContext = CreateDbContext();

        var trainingClass = await CreateFutureTrainingClassAsync(dbContext);

        var booking = new Booking
        {
            UserId = "user-1",
            TrainingClassId = trainingClass.Id,
            BookedAt = DateTime.UtcNow
        };

        await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();

        var service = CreateBookingService(dbContext);

        var bookings = await service.GetUserBookingsAsync("user-1");

        var userBooking = Assert.Single(bookings);

        Assert.Equal(booking.Id, userBooking.BookingId);
        Assert.Equal(trainingClass.Id, userBooking.TrainingClassId);
        Assert.Equal("Morning Yoga", userBooking.Name);
        Assert.Equal("Yoga", userBooking.Category);
        Assert.Equal("Anna Berg", userBooking.InstructorName);
        Assert.Equal(trainingClass.StartsAt, userBooking.StartsAt);
        Assert.Equal(trainingClass.EndsAt, userBooking.EndsAt);
    }

    private static BookingService CreateBookingService(CoreFitnessClubDbContext dbContext)
    {
        var bookingRepository = new BookingRepository(dbContext);
        var trainingClassRepository = new TrainingClassRepository(dbContext);
        var membershipRepository = new MembershipRepository(dbContext);

        return new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);
    }

    private static async Task<TrainingClass> CreateFutureTrainingClassAsync(CoreFitnessClubDbContext dbContext)
    {
        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1));

        await dbContext.TrainingClasses.AddAsync(trainingClass);
        await dbContext.SaveChangesAsync();

        return trainingClass;
    }

    private static async Task CreateActiveMembershipAsync(CoreFitnessClubDbContext dbContext, string userId)
    {
        var plan = new MembershipPlan
        {
            Name = "Basic",
            PricePerMonth = 299,
            ClassesPerMonth = 8,
            TrialWeeks = 2
        };

        await dbContext.MembershipPlans.AddAsync(plan);
        await dbContext.SaveChangesAsync();

        var membership = new Membership
        {
            UserId = userId,
            MembershipPlanId = plan.Id,
            Status = MembershipStatus.Active,
            StartDate = DateTime.UtcNow
        };

        await dbContext.Memberships.AddAsync(membership);
        await dbContext.SaveChangesAsync();
    }

    private static CoreFitnessClubDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CoreFitnessClubDbContext>()
            .UseInMemoryDatabase($"CoreFitnessClubTests-{Guid.NewGuid()}")
            .Options;

        return new CoreFitnessClubDbContext(options);
    }
}