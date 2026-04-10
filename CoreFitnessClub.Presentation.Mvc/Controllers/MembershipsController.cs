using CoreFitnessClub.Application.Features.Memberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

public class MembershipsController : Controller
{
    private readonly IReadMembershipService _readMembershipService;
    private readonly IMembershipService _membershipService;

    public MembershipsController(
        IReadMembershipService readMembershipService,
        IMembershipService membershipService)
    {
        _readMembershipService = readMembershipService;
        _membershipService = membershipService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var plans = await _readMembershipService.GetPlansAsync();

        return View(plans);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMembershipRequest request)
    {
        var result = await _membershipService.CreateMembershipAsync(request);

        if (!result.Succeeded)
        {
            TempData["MembershipError"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("MyMembership", "Account");
    }
}