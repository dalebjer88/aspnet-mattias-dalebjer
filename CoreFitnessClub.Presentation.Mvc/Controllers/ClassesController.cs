using System.Security.Claims;
using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Application.Features.Classes;
using CoreFitnessClub.Application.Features.Memberships;
using CoreFitnessClub.Presentation.Mvc.ViewModels.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize(Roles = "Member,Admin")]
public class ClassesController : Controller
{
    private readonly IReadTrainingClassService _readTrainingClassService;
    private readonly IBookingService _bookingService;
    private readonly IReadMembershipService _readMembershipService;

    public ClassesController(
        IReadTrainingClassService readTrainingClassService,
        IBookingService bookingService,
        IReadMembershipService readMembershipService)
    {
        _readTrainingClassService = readTrainingClassService;
        _bookingService = bookingService;
        _readMembershipService = readMembershipService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var classes = await _readTrainingClassService.GetAvailableAsync();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var bookedTrainingClassIds = new HashSet<int>();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            bookedTrainingClassIds = bookings
                .Select(x => x.TrainingClassId)
                .ToHashSet();
        }

        var hasActiveMembership = await _readMembershipService.HasActiveMembershipAsync();

        var model = new ClassesIndexViewModel
        {
            Classes = classes,
            BookedTrainingClassIds = bookedTrainingClassIds,
            HasActiveMembership = hasActiveMembership
        };

        return View(model);
    }
}