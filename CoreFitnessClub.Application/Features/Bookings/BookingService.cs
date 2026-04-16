using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Common.Results;
using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Features.Bookings;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ITrainingClassRepository _trainingClassRepository;
    private readonly IMembershipRepository _membershipRepository;

    public BookingService(
        IBookingRepository bookingRepository,
        ITrainingClassRepository trainingClassRepository,
        IMembershipRepository membershipRepository)
    {
        _bookingRepository = bookingRepository;
        _trainingClassRepository = trainingClassRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<Result> BookAsync(string userId, int trainingClassId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result.Failure("User not found.");
        }

        var membership = await _membershipRepository.GetByUserIdAsync(userId, cancellationToken);

        if (membership is null)
        {
            return Result.Failure("You need a membership to book a class.");
        }

        if (membership.Status != MembershipStatus.Active)
        {
            return Result.Failure("Your membership is not active.");
        }

        var trainingClass = await _trainingClassRepository.GetByIdAsync(trainingClassId, cancellationToken);

        if (trainingClass is null)
        {
            return Result.Failure("Class not found.");
        }

        var existingBooking = await _bookingRepository.GetByUserIdAndTrainingClassIdAsync(userId, trainingClassId, cancellationToken);

        if (existingBooking is not null)
        {
            return Result.Failure("You have already booked this class.");
        }

        var booking = new Booking
        {
            UserId = userId,
            TrainingClassId = trainingClassId,
            BookedAt = DateTime.UtcNow
        };

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> CancelAsync(string userId, int trainingClassId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Result.Failure("User not found.");
        }

        var existingBooking = await _bookingRepository.GetByUserIdAndTrainingClassIdAsync(userId, trainingClassId, cancellationToken);

        if (existingBooking is null)
        {
            return Result.Failure("Booking not found.");
        }

        _bookingRepository.Remove(existingBooking);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<List<UserBookingDto>> GetUserBookingsAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return [];
        }

        return await _bookingRepository.GetUserBookingsAsync(userId, cancellationToken);
    }
}