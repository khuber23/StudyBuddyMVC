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
		Uri baseAddress = new Uri("https://localhost:7025/api/");
		private readonly HttpClient _client;

		public StudySessionController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}

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

        [HttpGet("StartSession")]
        [Route("StartSession")]
        public IActionResult StartSession(int? pageNumber)
		{
			int pageSize = 1;

			List<FlashCard> flashcards = new List<FlashCard>();
			HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/FlashCard").Result;

			if(response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				flashcards = JsonConvert.DeserializeObject<List<FlashCard>>(data);
			}
			return View(PaginatedList<FlashCard>.Create(flashcards.ToList(), pageNumber ?? 1, pageSize));
		}
	}
}
