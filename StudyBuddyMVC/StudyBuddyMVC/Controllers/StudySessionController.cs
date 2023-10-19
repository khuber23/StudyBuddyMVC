﻿using ApiStudyBuddy.Data;
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

namespace StudyBuddyMVC.Controllers
{
	[Authorize]
	public class StudySessionController : Controller
	{
		Uri baseAddress = new Uri("https://localhost:7025/api/");
		private readonly HttpClient _client;

        public StudySessionController()
		{
			_client = new HttpClient();
			_client.BaseAddress = baseAddress;
		}

		[Authorize]
		[HttpGet("MySession")]
		[Route("MySession")]
		public IActionResult MySession()
		{
            List<UserDeckGroup> deckgroups = new List<UserDeckGroup>();
            HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/UserDeckGroup/user/1").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroups = JsonConvert.DeserializeObject<List<UserDeckGroup>>(data);
            }
            return View(deckgroups);
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
