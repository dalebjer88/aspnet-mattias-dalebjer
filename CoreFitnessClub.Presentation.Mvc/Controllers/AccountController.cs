using CoreFitnessClub.Application.Features.Account;
using CoreFitnessClub.Application.Features.Memberships;
using CoreFitnessClub.Infrastructure.Identity;
using CoreFitnessClub.Presentation.Mvc.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IReadMembershipService _readMembershipService;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(
        IAccountService accountService,
        IReadMembershipService readMembershipService,
        SignInManager<AppUser> signInManager)
    {
        _accountService = accountService;
        _readMembershipService = readMembershipService;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {
        var aboutMe = await _accountService.GetAboutMeAsync();

        if (aboutMe is null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var model = new AboutMeViewModel
        {
            Email = aboutMe.Email,
            FirstName = aboutMe.FirstName,
            LastName = aboutMe.LastName,
            PhoneNumber = aboutMe.PhoneNumber,
            ProfileImagePath = aboutMe.ProfileImagePath
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutMe(AboutMeViewModel model)
    {
        var aboutMe = await _accountService.GetAboutMeAsync();

        if (aboutMe is null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        model.Email = aboutMe.Email;
        model.ProfileImagePath = aboutMe.ProfileImagePath;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var request = new UpdateAboutMeRequest
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };

        await _accountService.SaveAboutMeAsync(request);

        return RedirectToAction(nameof(AboutMe));
    }

    [HttpGet]
    public IActionResult DeleteAccount()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAccountConfirmed()
    {
        var deleted = await _accountService.DeleteAccountAsync();

        if (!deleted)
        {
            TempData["DeleteAccountError"] = "Something went wrong while removing your account. Please try again.";
            return RedirectToAction(nameof(DeleteAccount));
        }

        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> MyMembership()
    {
        var myMembership = await _readMembershipService.GetMyMembershipAsync();

        var model = new MyMembershipViewModel();

        if (myMembership is not null)
        {
            model.PlanName = myMembership.PlanName;
            model.PricePerMonth = myMembership.PricePerMonth;
            model.ClassesPerMonth = myMembership.ClassesPerMonth;
            model.TrialWeeks = myMembership.TrialWeeks;
            model.Features = myMembership.Features;
            model.Status = myMembership.Status;
            model.StartDate = myMembership.StartDate;
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult MyBookings()
    {
        return View();
    }
}