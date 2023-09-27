using Microsoft.AspNetCore.Mvc;

namespace StudyBuddyMVC.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
