using System.Security.Claims;
using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Application.Features.Classes;
using CoreFitnessClub.Presentation.Mvc.ViewModels.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize]
public class ClassesController : Controller
{
    private readonly IReadTrainingClassService _readTrainingClassService;
    private readonly IBookingService _bookingService;

    public ClassesController(
        IReadTrainingClassService readTrainingClassService,
        IBookingService bookingService)
    {
        _readTrainingClassService = readTrainingClassService;
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _readTrainingClassService.GetAllAsync();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var bookedTrainingClassIds = new HashSet<int>();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            bookedTrainingClassIds = bookings
                .Select(x => x.TrainingClassId)
                .ToHashSet();
        }

        var model = new ClassesIndexViewModel
        {
            Classes = classes,
            BookedTrainingClassIds = bookedTrainingClassIds
        };

        return View(model);
    }
}