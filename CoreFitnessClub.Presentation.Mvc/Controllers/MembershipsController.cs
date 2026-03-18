using Microsoft.AspNetCore.Mvc;

namespace CoreFitnessClub.Presentation.Mvc.Controllers
{
    public class MembershipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
