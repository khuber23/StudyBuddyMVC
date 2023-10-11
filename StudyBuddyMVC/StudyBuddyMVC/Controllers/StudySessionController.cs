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
		Uri baseAddress = new Uri("https://localhost:7025/api/");
		private readonly HttpClient _client;
        public JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public StudySessionController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}

		[HttpGet("MySession")]
		[Route("MySession")]
		public IActionResult MySession()
		{
			List<UserDeckGroup> model = new List<UserDeckGroup>();
			HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/UserDeckGroup/user/1").Result;

			if (response.IsSuccessStatusCode)
			{
				string data = response.Content.ReadAsStringAsync().Result;
				model = System.Text.Json.JsonSerializer.Deserialize<List<UserDeckGroup>>(data, _serializerOptions);
                ViewBag.DeckGroups = model;
			}
			return View(model);
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

		public List<UserDeckGroup> UserDeckGroups { get; set; }
	}
}
