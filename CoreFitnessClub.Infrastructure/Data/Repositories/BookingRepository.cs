using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Application.Common.Exceptions;
using Microsoft.Data.SqlClient;
using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Infrastructure.Data.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly CoreFitnessClubDbContext _dbContext;

    public BookingRepository(CoreFitnessClubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Booking?> GetByUserIdAndTrainingClassIdAsync(string userId, int trainingClassId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TrainingClassId == trainingClassId, cancellationToken);
    }

    public async Task<List<UserBookingDto>> GetUserBookingsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Bookings
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Join(
                _dbContext.TrainingClasses.AsNoTracking(),
                booking => booking.TrainingClassId,
                trainingClass => trainingClass.Id,
                (booking, trainingClass) => new UserBookingDto
                {
                    BookingId = booking.Id,
                    TrainingClassId = trainingClass.Id,
                    Name = trainingClass.Name,
                    Category = trainingClass.Category,
                    InstructorName = trainingClass.InstructorName,
                    StartsAt = trainingClass.StartsAt,
                    EndsAt = trainingClass.EndsAt
                })
            .OrderBy(x => x.StartsAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await _dbContext.Bookings.AddAsync(booking, cancellationToken);
    }

    public void Remove(Booking booking)
    {
        _dbContext.Bookings.Remove(booking);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (IsUniqueConstraintViolation(exception))
        {
            throw new DuplicateEntityException("You have already booked this class.", exception);
        }
    }

    public async Task RemoveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var bookings = await _dbContext.Bookings
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        if (bookings.Count == 0)
        {
            return;
        }

        _dbContext.Bookings.RemoveRange(bookings);
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException &&
               sqlException.Number is 2601 or 2627;
    }
}