namespace CoreFitnessClub.Application.Features.Classes;

public interface IReadTrainingClassService
{
    Task<List<TrainingClassDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<TrainingClassDto>> GetAvailableAsync(CancellationToken cancellationToken = default);
}