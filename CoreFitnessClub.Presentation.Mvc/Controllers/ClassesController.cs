using CoreFitnessClub.Application.Features.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize]
public class ClassesController : Controller
{
    private readonly IReadTrainingClassService _readTrainingClassService;

    public ClassesController(IReadTrainingClassService readTrainingClassService)
    {
        _readTrainingClassService = readTrainingClassService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _readTrainingClassService.GetAllAsync();
        return View(classes);
    }
}