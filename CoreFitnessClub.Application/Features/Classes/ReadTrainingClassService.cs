using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Application.Features.Classes;

public class ReadTrainingClassService : IReadTrainingClassService
{
    private readonly ITrainingClassRepository _trainingClassRepository;

    public ReadTrainingClassService(ITrainingClassRepository trainingClassRepository)
    {
        _trainingClassRepository = trainingClassRepository;
    }

    public async Task<List<TrainingClassDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var trainingClasses = await _trainingClassRepository.GetAllAsync(cancellationToken);

        return trainingClasses.Select(MapToDto).ToList();
    }

    public async Task<List<TrainingClassDto>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        var trainingClasses = await _trainingClassRepository.GetAvailableAsync(DateTime.Now, cancellationToken);

        return trainingClasses.Select(MapToDto).ToList();
    }

    private static TrainingClassDto MapToDto(TrainingClass trainingClass)
    {
        return new TrainingClassDto
        {
            Id = trainingClass.Id,
            Name = trainingClass.Name,
            Category = trainingClass.Category,
            InstructorName = trainingClass.InstructorName,
            StartsAt = trainingClass.StartsAt,
            EndsAt = trainingClass.EndsAt
        };
    }
}