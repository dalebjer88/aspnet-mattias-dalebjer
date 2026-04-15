using CoreFitnessClub.Application.Features.Classes;
using CoreFitnessClub.Presentation.Mvc.Areas.Admin.ViewModels.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ClassesController : Controller
{
    private readonly IReadTrainingClassService _readTrainingClassService;
    private readonly IManageTrainingClassService _manageTrainingClassService;

    public ClassesController(
        IReadTrainingClassService readTrainingClassService,
        IManageTrainingClassService manageTrainingClassService)
    {
        _readTrainingClassService = readTrainingClassService;
        _manageTrainingClassService = manageTrainingClassService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _readTrainingClassService.GetAllAsync();

        var model = new AdminClassesIndexViewModel
        {
            Classes = classes
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminClassesIndexViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Classes = await _readTrainingClassService.GetAllAsync();
            return View("Index", model);
        }

        var request = new CreateTrainingClassRequest
        {
            Name = model.Form.Name,
            Category = model.Form.Category,
            InstructorName = model.Form.InstructorName,
            Date = model.Form.Date!.Value,
            StartTime = model.Form.StartTime!.Value,
            EndTime = model.Form.EndTime!.Value
        };

        var result = await _manageTrainingClassService.CreateAsync(request);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Unable to create class.");
            model.Classes = await _readTrainingClassService.GetAllAsync();
            return View("Index", model);
        }

        TempData["SuccessMessage"] = "Class created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _manageTrainingClassService.DeleteAsync(id);

        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded ? "Class deleted successfully." : result.ErrorMessage;

        return RedirectToAction(nameof(Index));
    }
}