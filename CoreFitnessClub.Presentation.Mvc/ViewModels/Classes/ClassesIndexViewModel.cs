namespace CoreFitnessClub.Presentation.Mvc.ViewModels.Classes;

using CoreFitnessClub.Application.Features.Classes;

public class ClassesIndexViewModel
{
    public List<TrainingClassDto> Classes { get; set; } = [];
    public HashSet<int> BookedTrainingClassIds { get; set; } = [];
    public bool HasActiveMembership { get; set; }
}