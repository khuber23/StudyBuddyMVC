using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyBuddyMVC.Controllers
{
    public class MyDashboardController : Controller
    {
        [Authorize]
        [HttpGet("MyStats")]
        [Route("MyStats")]
        public IActionResult MyStats()
        {
            return View();
        }

		[Authorize]
		[HttpGet("MyHistory")]
        [Route("MyHistory")]
        public IActionResult MyHistory()
        {
            return View();
        }

		[Authorize]
		[HttpGet("MyStudyPriority")]
        [Route("MyStudyPriority")]
        public IActionResult MyStudyPriority()
        {
            return View();
        }
    }
}
