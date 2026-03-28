using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AboutMe()
        {
            return View();
        }

        public IActionResult MyMembership()
        {
            return View();
        }

        public IActionResult MyBookings()
        {
            return View();
        }
    }
}