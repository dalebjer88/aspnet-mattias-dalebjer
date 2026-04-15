using CoreFitnessClub.Application.Common.Results;

namespace CoreFitnessClub.Application.Features.Classes;

public interface IManageTrainingClassService
{
    Task<Result> CreateAsync(CreateTrainingClassRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
}