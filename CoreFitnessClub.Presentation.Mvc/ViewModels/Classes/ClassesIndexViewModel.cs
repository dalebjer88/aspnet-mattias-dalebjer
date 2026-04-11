using CoreFitnessClub.Application.Features.Classes;

namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Classes;

public class ClassesIndexViewModel
{
    public List<TrainingClassDto> Classes { get; set; } = [];
    public HashSet<int> BookedTrainingClassIds { get; set; } = [];
}