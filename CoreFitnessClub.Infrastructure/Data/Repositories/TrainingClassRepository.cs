using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Infrastructure.Data.Repositories;

public class TrainingClassRepository : ITrainingClassRepository
{
    private readonly CoreFitnessClubDbContext _dbContext;

    public TrainingClassRepository(CoreFitnessClubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TrainingClass>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.TrainingClasses
            .OrderBy(x => x.StartsAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<TrainingClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TrainingClasses
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(TrainingClass trainingClass, CancellationToken cancellationToken = default)
    {
        await _dbContext.TrainingClasses.AddAsync(trainingClass, cancellationToken);
    }

    public void Remove(TrainingClass trainingClass)
    {
        _dbContext.TrainingClasses.Remove(trainingClass);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}