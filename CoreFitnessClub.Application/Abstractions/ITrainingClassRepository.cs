using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Abstractions;

public interface ITrainingClassRepository
{
    Task<List<TrainingClass>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TrainingClass?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}