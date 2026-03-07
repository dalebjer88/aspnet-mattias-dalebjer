using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Membership()
        {
            return View();
        }

        public IActionResult CustomerService()
        {
            return View();
        }
    }
}
