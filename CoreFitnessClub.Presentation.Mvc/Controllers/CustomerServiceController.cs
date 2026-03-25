using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class CustomerServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}