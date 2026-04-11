using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Abstractions;

public interface IBookingRepository
{
    Task<Booking?> GetByUserIdAndTrainingClassIdAsync(string userId, int trainingClassId, CancellationToken cancellationToken = default);
    Task<List<UserBookingDto>> GetUserBookingsAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    void Remove(Booking booking);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}