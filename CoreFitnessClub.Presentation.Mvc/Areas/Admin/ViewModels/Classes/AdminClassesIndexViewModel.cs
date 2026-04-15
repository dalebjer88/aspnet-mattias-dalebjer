using CoreFitnessClub.Application.Features.Classes;

namespace CoreFitnessClub.Presentation.Mvc.Areas.Admin.ViewModels.Classes;

public class AdminClassesIndexViewModel
{
    public CreateTrainingClassViewModel Form { get; set; } = new();
    public List<TrainingClassDto> Classes { get; set; } = [];
}