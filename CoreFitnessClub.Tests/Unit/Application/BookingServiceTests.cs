using System.Reflection;
using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Domain.Entities;
using NSubstitute;

namespace CoreFitnessClub.Tests.Unit.Application;

public class BookingServiceTests
{
    [Fact]
    public async Task BookAsync_WithPendingMembership_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(new Membership
            {
                UserId = "user-1",
                MembershipPlanId = 1,
                Status = MembershipStatus.Pending,
                StartDate = DateTime.UtcNow
            });

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("Your membership is not active.", result.ErrorMessage);

        await trainingClassRepository.DidNotReceive().GetByIdAsync(
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookAsync_WithoutMembership_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns((Membership?)null);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("You need a membership to book a class.", result.ErrorMessage);

        await trainingClassRepository.DidNotReceive().GetByIdAsync(
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookAsync_WithInactiveMembership_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(new Membership
            {
                UserId = "user-1",
                MembershipPlanId = 1,
                Status = MembershipStatus.Pending,
                StartDate = DateTime.UtcNow
            });

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("Your membership is not active.", result.ErrorMessage);

        await trainingClassRepository.DidNotReceive().GetByIdAsync(
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookAsync_WhenClassDoesNotExist_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(CreateActiveMembership("user-1"));

        trainingClassRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((TrainingClass?)null);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("Class not found.", result.ErrorMessage);

        await bookingRepository.DidNotReceive().AddAsync(
            Arg.Any<Booking>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookAsync_WhenClassHasAlreadyStarted_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(CreateActiveMembership("user-1"));

        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddHours(-2),
            DateTime.Now.AddHours(-1));

        SetEntityId(trainingClass, 1);

        trainingClassRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(trainingClass);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("You cannot book a class that has already started.", result.ErrorMessage);

        await bookingRepository.DidNotReceive().AddAsync(
            Arg.Any<Booking>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookAsync_WhenAlreadyBooked_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(CreateActiveMembership("user-1"));

        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1));

        SetEntityId(trainingClass, 1);

        trainingClassRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(trainingClass);

        bookingRepository.GetByUserIdAndTrainingClassIdAsync("user-1", 1, Arg.Any<CancellationToken>())
            .Returns(new Booking
            {
                UserId = "user-1",
                TrainingClassId = 1,
                BookedAt = DateTime.UtcNow
            });

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("You have already booked this class.", result.ErrorMessage);

        await bookingRepository.DidNotReceive().AddAsync(
            Arg.Any<Booking>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookAsync_WithValidData_ReturnsSuccess()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        membershipRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(CreateActiveMembership("user-1"));

        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1));

        SetEntityId(trainingClass, 1);

        trainingClassRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(trainingClass);

        bookingRepository.GetByUserIdAndTrainingClassIdAsync("user-1", 1, Arg.Any<CancellationToken>())
            .Returns((Booking?)null);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.BookAsync("user-1", 1);

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);

        await bookingRepository.Received(1).AddAsync(
            Arg.Is<Booking>(booking =>
                booking.UserId == "user-1" &&
                booking.TrainingClassId == 1),
            Arg.Any<CancellationToken>());

        await bookingRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelAsync_WithEmptyUserId_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.CancelAsync("", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("User not found.", result.ErrorMessage);

        await bookingRepository.DidNotReceive().GetByUserIdAndTrainingClassIdAsync(
            Arg.Any<string>(),
            Arg.Any<int>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelAsync_WhenBookingDoesNotExist_ReturnsFailure()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        bookingRepository.GetByUserIdAndTrainingClassIdAsync("user-1", 1, Arg.Any<CancellationToken>())
            .Returns((Booking?)null);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.CancelAsync("user-1", 1);

        Assert.False(result.Succeeded);
        Assert.Equal("Booking not found.", result.ErrorMessage);

        bookingRepository.DidNotReceive().Remove(Arg.Any<Booking>());
        await bookingRepository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelAsync_WhenBookingExists_ReturnsSuccess()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        var booking = new Booking
        {
            UserId = "user-1",
            TrainingClassId = 1,
            BookedAt = DateTime.UtcNow
        };

        bookingRepository.GetByUserIdAndTrainingClassIdAsync("user-1", 1, Arg.Any<CancellationToken>())
            .Returns(booking);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var result = await service.CancelAsync("user-1", 1);

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);

        bookingRepository.Received(1).Remove(booking);
        await bookingRepository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserBookingsAsync_WithEmptyUserId_ReturnsEmptyList()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var bookings = await service.GetUserBookingsAsync("");

        Assert.Empty(bookings);

        await bookingRepository.DidNotReceive().GetUserBookingsAsync(
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUserBookingsAsync_WithUserId_ReturnsBookingsFromRepository()
    {
        var bookingRepository = Substitute.For<IBookingRepository>();
        var trainingClassRepository = Substitute.For<ITrainingClassRepository>();
        var membershipRepository = Substitute.For<IMembershipRepository>();

        var expectedBookings = new List<UserBookingDto>
        {
            new()
            {
                BookingId = 1,
                TrainingClassId = 1,
                Name = "Morning Yoga",
                Category = "Yoga",
                InstructorName = "Anna Berg",
                StartsAt = new DateTime(2026, 5, 3, 10, 0, 0),
                EndsAt = new DateTime(2026, 5, 3, 11, 0, 0)
            }
        };

        bookingRepository.GetUserBookingsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(expectedBookings);

        var service = new BookingService(
            bookingRepository,
            trainingClassRepository,
            membershipRepository);

        var bookings = await service.GetUserBookingsAsync("user-1");

        Assert.Single(bookings);
        Assert.Equal(expectedBookings, bookings);

        await bookingRepository.Received(1).GetUserBookingsAsync(
            "user-1",
            Arg.Any<CancellationToken>());
    }

    private static Membership CreateActiveMembership(string userId)
    {
        return new Membership
        {
            UserId = userId,
            MembershipPlanId = 1,
            Status = MembershipStatus.Active,
            StartDate = DateTime.UtcNow
        };
    }

    private static void SetEntityId(BaseEntity entity, int id)
    {
        var property = typeof(BaseEntity).GetProperty(
            nameof(BaseEntity.Id),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        property!.SetValue(entity, id);
    }
}