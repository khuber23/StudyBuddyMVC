using Microsoft.AspNetCore.Mvc;

namespace StudyBuddyMVC.Controllers
{
	public class StudySessionController : Controller
	{
		[HttpGet("MySession")]
		[Route("MySession")]
		public IActionResult MySession()
		{
			return View();
		}

		[HttpGet("StudyPriority")]
		[Route("StudyPriority")]
		public IActionResult StudyPriority()
		{
			return View();
		}
	}
}
