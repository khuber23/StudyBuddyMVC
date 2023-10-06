using Microsoft.AspNetCore.Mvc;

namespace StudyBuddyMVC.Controllers
{
    public class MyDashboardController : Controller
    {
        [HttpGet("MyStats")]
        [Route("MyStats")]
        public IActionResult MyStats()
        {
            return View();
        }

        [HttpGet("MyHistory")]
        [Route("MyHistory")]
        public IActionResult MyHistory()
        {
            return View();
        }

        [HttpGet("MyStudyPriority")]
        [Route("MyStudyPriority")]
        public IActionResult MyStudyPriority()
        {
            return View();
        }
    }
}
