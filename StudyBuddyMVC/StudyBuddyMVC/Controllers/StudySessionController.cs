using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using System.Net.Http.Headers;
using System.Text;

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
