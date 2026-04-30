using CoreFitnessClub.Presentation.Mvc.ViewModels.Memberships;
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

    [Authorize(Roles = "Member")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMembershipViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["MembershipError"] = ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .FirstOrDefault() ?? "Unable to create membership.";

            return RedirectToAction(nameof(Index));
        }

        var request = new CreateMembershipRequest
        {
            MembershipPlanId = model.MembershipPlanId
        };

        var result = await _membershipService.CreateMembershipAsync(request);

        if (!result.Succeeded)
        {
            TempData["MembershipError"] = result.ErrorMessage ?? "Unable to create membership.";
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("MyMembership", "Account");
    }
}