using System.Security.Claims;
using CoreFitnessClub.Application.Features.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize]
public class BookingsController : Controller
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(int trainingClassId, string? returnUrl)
    {
        if (User.IsInRole("Admin"))
        {
            TempData["ErrorMessage"] = "Admin accounts cannot book classes.";
            return RedirectToLocalOrDefault(returnUrl);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var result = await _bookingService.BookAsync(userId, trainingClassId);

        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded ? "Class booked successfully." : result.ErrorMessage;

        return RedirectToLocalOrDefault(returnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int trainingClassId, string? returnUrl)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var result = await _bookingService.CancelAsync(userId, trainingClassId);

        TempData[result.Succeeded ? "SuccessMessage" : "ErrorMessage"] =
            result.Succeeded ? "Booking cancelled successfully." : result.ErrorMessage;

        return RedirectToLocalOrDefault(returnUrl);
    }

    private IActionResult RedirectToLocalOrDefault(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Classes");
    }
}