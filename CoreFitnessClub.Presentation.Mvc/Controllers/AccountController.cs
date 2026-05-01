using CoreFitnessClub.Application.Features.Account;
using CoreFitnessClub.Application.Features.Memberships;
using CoreFitnessClub.Infrastructure.Identity;
using CoreFitnessClub.Presentation.Mvc.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CoreFitnessClub.Application.Features.Bookings;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

[Authorize(Roles = "Member,Admin")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IReadMembershipService _readMembershipService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IBookingService _bookingService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(
        IAccountService accountService,
        IReadMembershipService readMembershipService,
        IBookingService bookingService,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment)
    {
        _accountService = accountService;
        _readMembershipService = readMembershipService;
        _bookingService = bookingService;
        _signInManager = signInManager;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {
        var aboutMe = await _accountService.GetAboutMeAsync();

        if (aboutMe is null)
        {
            return RedirectToAction("SignIn", "Auth");
        }

        ViewData["AccountTab"] = "AboutMe";
        ViewData["ProfileImagePath"] = aboutMe.ProfileImagePath;

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
            ViewData["AccountTab"] = "AboutMe";
            ViewData["ProfileImagePath"] = model.ProfileImagePath;
            return View(model);
        }

        var oldProfileImagePath = aboutMe.ProfileImagePath;
        var profileImagePath = aboutMe.ProfileImagePath;
        var hasNewProfileImage = model.ProfileImage is not null && model.ProfileImage.Length > 0;

        if (hasNewProfileImage)
        {
            var extension = Path.GetExtension(model.ProfileImage!.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(model.ProfileImage), "Only image files are allowed.");
                ViewData["AccountTab"] = "AboutMe";
                ViewData["ProfileImagePath"] = model.ProfileImagePath;
                return View(model);
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profile-images");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.ProfileImage.CopyToAsync(stream);

            profileImagePath = $"/uploads/profile-images/{fileName}";
        }
        var request = new UpdateAboutMeRequest
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            ProfileImagePath = profileImagePath
        };

        await _accountService.SaveAboutMeAsync(request);

        if (hasNewProfileImage)
        {
            DeleteProfileImageFile(oldProfileImagePath);
        }

        return RedirectToAction(nameof(AboutMe));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteAccount()
    {
        await SetAccountLayoutDataAsync("", "Delete Account");

        return View();
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var aboutMe = await _accountService.GetAboutMeAsync();
            var profileImagePath = aboutMe?.ProfileImagePath;

            var deleted = await _accountService.DeleteAccountAsync();

            if (!deleted)
            {
                TempData["DeleteAccountError"] = "Something went wrong while removing your account. Please try again.";
                return RedirectToAction(nameof(DeleteAccount));
            }

            await _signInManager.SignOutAsync();

            DeleteProfileImageFile(profileImagePath);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
    public async Task<IActionResult> MyMembership()
    {
        await SetAccountLayoutDataAsync("MyMembership", "My Membership");

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
    public async Task<IActionResult> MyBookings()
    {
        await SetAccountLayoutDataAsync("MyBookings", "My Bookings");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return RedirectToAction("SignIn", "Auth");
        }

        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return View(bookings);
    }

        private async Task SetAccountLayoutDataAsync(string accountTab, string accountTitle)
    {
        ViewData["AccountTab"] = accountTab;
        ViewData["AccountTitle"] = accountTitle;

        var aboutMe = await _accountService.GetAboutMeAsync();
        ViewData["ProfileImagePath"] = aboutMe?.ProfileImagePath;
    }
    private void DeleteProfileImageFile(string? profileImagePath)
    {
        if (string.IsNullOrWhiteSpace(profileImagePath))
        {
            return;
        }

        var normalizedPath = profileImagePath.Replace('\\', '/');

        if (normalizedPath.StartsWith("~/", StringComparison.Ordinal))
        {
            normalizedPath = normalizedPath[1..];
        }

        const string profileImagesPath = "/uploads/profile-images/";

        if (!normalizedPath.StartsWith(profileImagesPath, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var fileName = Path.GetFileName(normalizedPath);

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return;
        }

        var filePath = Path.Combine(
            _webHostEnvironment.WebRootPath,
            "uploads",
            "profile-images",
            fileName);

        try
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}