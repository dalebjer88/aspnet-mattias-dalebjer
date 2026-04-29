using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Abstractions;

public interface ITrainingClassRepository
{
    Task<List<TrainingClass>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<TrainingClass>> GetAvailableAsync(DateTime currentDateTime, CancellationToken cancellationToken = default);
    Task<TrainingClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(TrainingClass trainingClass, CancellationToken cancellationToken = default);
    void Remove(TrainingClass trainingClass);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}