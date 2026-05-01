using CoreFitnessClub.Presentation.Mvc.ViewModels.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers;

public class CustomerServiceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactRequestViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

            TempData["ContactMessageSuccess"] = "Your message was submitted successfully.";

        return RedirectToAction(nameof(Index));
    }
}