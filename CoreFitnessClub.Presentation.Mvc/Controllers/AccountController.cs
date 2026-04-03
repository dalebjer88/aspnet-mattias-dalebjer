using CoreFitnessClub.Application.Features.Account;
using CoreFitnessClub.Presentation.Mvc.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
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
    public IActionResult MyMembership()
    {
        return View();
    }

    [HttpGet]
    public IActionResult MyBookings()
    {
        return View();
    }
}