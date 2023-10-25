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
		Uri baseAddress = new Uri("https://localhost:7025/api/");
		private readonly HttpClient _client;
		private readonly IUserService _userService;

        public StudySessionController(IUserService userService)
		{
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
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
		[HttpGet("MySession")]
		[Route("MySession")]
		public IActionResult MySession()
		{
			User user = new User();
			var userid = _userService.GetUserId();

			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri("https://localhost:7025/api/User/");
				var response = httpClient.GetAsync("{id}?userid=" + userid);
				response.Wait();
				var result = response.Result;
				if (result.IsSuccessStatusCode)
				{
					string data = result.Content.ReadAsStringAsync().Result;
					user = JsonConvert.DeserializeObject<User>(data);
				}
			}
			return View(user);
		}


		[Authorize]
		[HttpGet("StudyPriority")]
		[Route("StudyPriority")]
		public IActionResult StudyPriority()
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
