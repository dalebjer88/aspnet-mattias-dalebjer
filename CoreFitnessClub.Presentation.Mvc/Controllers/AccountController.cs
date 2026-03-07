using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
