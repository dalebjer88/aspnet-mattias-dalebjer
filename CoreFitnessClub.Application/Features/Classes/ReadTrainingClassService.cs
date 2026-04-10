using CoreFitnessClub.Application.Abstractions;

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

        return trainingClasses.Select(x => new TrainingClassDto
        {
            Id = x.Id,
            Name = x.Name,
            Category = x.Category,
            InstructorName = x.InstructorName,
            StartsAt = x.StartsAt,
            EndsAt = x.EndsAt
        }).ToList();
    }
}