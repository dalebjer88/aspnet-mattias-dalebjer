namespace CoreFitnessClub.Application.Features.Classes;

public interface IReadTrainingClassService
{
    Task<List<TrainingClassDto>> GetAllAsync(CancellationToken cancellationToken = default);
}