using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}