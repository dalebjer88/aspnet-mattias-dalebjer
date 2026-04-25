using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Common.Results;
using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Domain.Exceptions;

namespace CoreFitnessClub.Application.Features.Classes;

public class ManageTrainingClassService : IManageTrainingClassService
{
    private readonly ITrainingClassRepository _trainingClassRepository;

    public ManageTrainingClassService(ITrainingClassRepository trainingClassRepository)
    {
        _trainingClassRepository = trainingClassRepository;
    }

    public async Task<Result> CreateAsync(CreateTrainingClassRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Result.Failure("Class name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            return Result.Failure("Category is required.");
        }

        if (string.IsNullOrWhiteSpace(request.InstructorName))
        {
            return Result.Failure("Instructor name is required.");
        }

        try
        {
            var startsAt = request.Date.ToDateTime(request.StartTime);
            var endsAt = request.Date.ToDateTime(request.EndTime);

            var trainingClass = TrainingClass.Create(
                request.Name.Trim(),
                request.Category.Trim(),
                request.InstructorName.Trim(),
                startsAt,
                endsAt);

            await _trainingClassRepository.AddAsync(trainingClass, cancellationToken);
            await _trainingClassRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex) when (ex is InvalidTrainingClassTimeException)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainingClass = await _trainingClassRepository.GetByIdAsync(id, cancellationToken);

        if (trainingClass is null)
        {
            return Result.Failure("Class not found.");
        }

        _trainingClassRepository.Remove(trainingClass);
        await _trainingClassRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}