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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using StudyBuddyMVC.Service;

namespace StudyBuddyMVC.Controllers
{
	[Authorize]
	public class StudySessionController : Controller
	{
		private readonly HttpClient _client;
		private readonly IUserService _userService;

        public StudySessionController(IUserService userService)
		{
            _client = new HttpClient();
            _userService = userService;
		}

		[Authorize]
		[HttpGet("MyStudySession")]
		[Route("MyStudySession")]
		public IActionResult MyStudySession()
		{
			return View();
		}


		[Authorize]
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
