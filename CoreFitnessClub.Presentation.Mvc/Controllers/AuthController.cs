using CoreFitnessClub.Infrastructure.Identity;
using CoreFitnessClub.Presentation.Mvc.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger= logger;
    }

    [HttpGet]
    public async Task<IActionResult> SignUp(string? returnUrl = null)
    {
        var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();

        var vm = new SignUpViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = [.. schemes.Select(x => x.Name)]
        };

        return View(vm);
    }
    private async Task PopulateExternalProvidersAsync(SignUpViewModel model)
    {
        var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
        model.ExternalProviders = [.. schemes.Select(x => x.Name)];
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateExternalProvidersAsync(model);
            return View(model);
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser is not null)
        {
            var hasPassword = await _userManager.HasPasswordAsync(existingUser);

            if (hasPassword)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                await PopulateExternalProvidersAsync(model);
                return View(model);
            }
        }

        TempData["PendingSignUpEmail"] = model.Email;

        return RedirectToAction(nameof(VerifySignUpEmail));
    }

    [HttpGet]
    public IActionResult VerifySignUpEmail()
    {
        if (TempData["PendingSignUpEmail"] is not string email || string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(SignUp));
        }

        TempData["PendingSignUpEmail"] = email;

        return View(new VerifySignUpEmailViewModel
        {
            Email = email
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult VerifySignUpEmail(VerifySignUpEmailViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!string.Equals(model.Code, "123456", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(model.Code), "Invalid verification code.");
            return View(model);
        }

        TempData["VerifiedSignUpEmail"] = model.Email;

        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet]
    public IActionResult SetPassword()
    {
        if (TempData["VerifiedSignUpEmail"] is not string email || string.IsNullOrWhiteSpace(email))
        {
            return RedirectToAction(nameof(SignUp));
        }

        TempData["VerifiedSignUpEmail"] = email;

        return View(new SetPasswordViewModel
        {
            Email = email
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (TempData["VerifiedSignUpEmail"] is not string verifiedEmail ||
            string.IsNullOrWhiteSpace(verifiedEmail) ||
            !string.Equals(verifiedEmail, model.Email, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction(nameof(SignUp));
        }

        if (!ModelState.IsValid)
        {
            TempData["VerifiedSignUpEmail"] = model.Email;
            return View(model);
        }

        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        IdentityResult passwordResult;
        AppUser user;

        if (existingUser is null)
        {
            user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            passwordResult = await _userManager.CreateAsync(user, model.Password);
        }
        else
        {
            var hasPassword = await _userManager.HasPasswordAsync(existingUser);

            if (hasPassword)
            {
                ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");
                TempData["VerifiedSignUpEmail"] = model.Email;
                return View(model);
            }

            if (!existingUser.EmailConfirmed)
            {
                existingUser.EmailConfirmed = true;

                var updateResult = await _userManager.UpdateAsync(existingUser);

                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData["VerifiedSignUpEmail"] = model.Email;
                    return View(model);
                }
            }

            user = existingUser;
            passwordResult = await _userManager.AddPasswordAsync(user, model.Password);
        }

        if (!passwordResult.Succeeded)
        {
            foreach (var error in passwordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            TempData["VerifiedSignUpEmail"] = model.Email;
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> SignIn(string? returnUrl = null)
    {
        var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();

        var vm = new SignInViewModel
        {
            ReturnUrl = returnUrl,
            ExternalProviders = [.. schemes.Select(x => x.Name)]
        };

        ViewData["ReturnUrl"] = returnUrl;
        return View(vm);
    }



    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return RedirectToAction(nameof(SignIn), new {returnUrl});
        
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return Challenge(properties, provider);
    }




    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError is not null)
        {
            _logger.LogWarning("Remote error during external login: {RemoteError}", remoteError);
            return ExternalLoginFailed(returnUrl);
        }

        var externalUser = await GetExternalUserInfo();
        if (externalUser is null)
            return ExternalLoginFailed(returnUrl);

        var (info, email) = externalUser.Value;

        var result = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (result.Succeeded)
            return RedirectToLocal(returnUrl);

        if (result.IsLockedOut)
        {
            TempData["ExternalLoginError"] = "This account is locked.";
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }

        if (result.IsNotAllowed)
        {
            TempData["ExternalLoginError"] = "This account is not allowed to sign in.";
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }

        return await ExternalVerification(email, returnUrl);
    }




    private async Task<IActionResult> ExternalVerification(string email, string? returnUrl = null)
    {
        return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
        {
            ReturnUrl = returnUrl,
            Email = email
        });
    }


#if DEBUG
    [HttpGet]
    public IActionResult TestVerifyExternalLogin()
    {
        return View("VerifyExternalLogin", new VerifyExternalLoginViewModel
        {
            Email = "test@domain.com",
            ReturnUrl = "/"
        });
    }

#endif


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyExternalLogin(VerifyExternalLoginViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("VerifyExternalLogin", vm);
      
        // TODO: Validera koden mot databas/cache

        if(!string.Equals(vm.Code, "123456", StringComparison.Ordinal))
        {
            ModelState.AddModelError(nameof(vm.Code), "Invalid verification code.");
            return View("VerifyExternalLogin", vm);
        }

        var externalUser = await GetExternalUserInfo();
        if (externalUser is null)
            return ExternalLoginFailed(vm.ReturnUrl);

        var (info, email) = externalUser.Value;

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return await LinkExistingUser(existingUser, info, vm.ReturnUrl);

        return await CreateExternalUser(email, info, vm.ReturnUrl);
    }


    private async Task<IActionResult> LinkExistingUser(AppUser user, ExternalLoginInfo info, string? returnUrl = null)
    {
        var alreadyLinkedUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (alreadyLinkedUser is not null)
        {
            if (alreadyLinkedUser.Id == user.Id)
            {
                await _signInManager.SignInAsync(alreadyLinkedUser, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }

            _logger.LogWarning(
                "External login {Provider}/{ProviderKey} is already linked to another user.",
                info.LoginProvider,
                info.ProviderKey);

            return ExternalLoginFailed(returnUrl);
        }

        if (!user.EmailConfirmed)
        {
            user.EmailConfirmed = true;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to confirm email for {Email}. Errors: {Errors}",
                    user.Email,
                    string.Join(",", updateResult.Errors.Select(x => x.Description)));

                return ExternalLoginFailed(returnUrl);
            }
        }

        var result = await _userManager.AddLoginAsync(user, info);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to link {Provider} to {Email}. Errors: {Errors}",
                info.LoginProvider,
                user.Email,
                string.Join(",", result.Errors.Select(x => x.Description)));

            return ExternalLoginFailed(returnUrl);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
    }

    private async Task<IActionResult> CreateExternalUser(string email, ExternalLoginInfo info, string? returnUrl = null) 
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true 
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Failed to create user for {Email}. Errors: {Errors}",
                email,
                string.Join(",", createResult.Errors.Select(x => x.Description))
            );
            return ExternalLoginFailed(returnUrl);
        }

        user = await _userManager.FindByIdAsync(user.Id);

        if (user is null)
        {
            _logger.LogError("Failed to reload created user for {Email}.", email);
            return ExternalLoginFailed(returnUrl);
        }

        var linkResult = await _userManager.AddLoginAsync(user, info);

        if (!linkResult.Succeeded)
        {
            _logger.LogError("Failed to link {Provider} to {Email}. Errors: {Errors}",
                info.LoginProvider,
                user.Email,
                string.Join(",", linkResult.Errors.Select(x => x.Description))
                );
            return ExternalLoginFailed(returnUrl);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToLocal(returnUrl);
    }

    private async Task<(ExternalLoginInfo info, string Email)?> GetExternalUserInfo() 
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            _logger.LogWarning("External login info was null.");
            return null;
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("Email claim not found in external login info.");
            return null;
        }
        return (info, email);
    }

    private RedirectToActionResult ExternalLoginFailed(string? returnUrl = null)
    {
        TempData["ExternalLoginError"] = "An error occurred while processing your external login. Please try again.";
        return RedirectToAction(nameof(SignIn), new { returnUrl });
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return RedirectToAction("SignIn", "Auth");
    }
}