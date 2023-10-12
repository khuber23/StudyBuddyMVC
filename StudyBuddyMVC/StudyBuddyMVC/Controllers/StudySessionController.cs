using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.VisualBasic;

namespace StudyBuddyMVC.Controllers
{
	public class StudySessionController : Controller
	{
		private readonly HttpClient _client;

        public StudySessionController()
		{
			_client = new HttpClient();
		}

		[HttpGet("MySession")]
		[Route("MySession")]
		public IActionResult MySession()
		{
            List<UserDeckGroup> deckgroups = new List<UserDeckGroup>();
            HttpResponseMessage response = _client.GetAsync("https://instruct.ntc.edu/studybuddyapi/api/userdeckgroup/user/1").Result;
			// https://localhost:7025/api/UserDeckGroup/user/1
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroups = JsonConvert.DeserializeObject<List<UserDeckGroup>>(data);
            }
            return View(deckgroups);
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
			HttpResponseMessage response = _client.GetAsync("https://instruct.ntc.edu/studybuddyapi/api/flashcard").Result;
            // https://localhost:7025/api/FlashCard
            if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				flashcards = JsonConvert.DeserializeObject<List<FlashCard>>(data);
			}
			return View(PaginatedList<FlashCard>.Create(flashcards.ToList(), pageNumber ?? 1, pageSize));
		}
	}
}
