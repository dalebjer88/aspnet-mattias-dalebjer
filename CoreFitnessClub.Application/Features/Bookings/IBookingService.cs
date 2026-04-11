using CoreFitnessClub.Application.Common.Results;

namespace CoreFitnessClub.Application.Features.Bookings;

public interface IBookingService
{
    Task<Result> BookAsync(string userId, int trainingClassId, CancellationToken cancellationToken = default);
    Task<Result> CancelAsync(string userId, int trainingClassId, CancellationToken cancellationToken = default);
    Task<List<UserBookingDto>> GetUserBookingsAsync(string userId, CancellationToken cancellationToken = default);
}