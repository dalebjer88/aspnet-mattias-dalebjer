using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
