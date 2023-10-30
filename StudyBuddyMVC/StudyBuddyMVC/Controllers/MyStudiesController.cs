using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using System.Text;

namespace StudyBuddyMVC.Controllers
{
	[Authorize]
	public class MyStudiesController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client;

        public MyStudiesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

		[Authorize]
		[HttpGet("MyDashboard")]
        [Route("MyDashboard")]
        public IActionResult MyDashboard()
        {
            return View();
        }

		[Authorize]
		[HttpGet("Flashcards")]
        [Route("Flashcards")]
        public IActionResult Flashcards()
        {
            List<FlashCard> flashcards = new List<FlashCard>();
            HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/FlashCard").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                flashcards = JsonConvert.DeserializeObject<List<FlashCard>>(data);
            }
            return View(flashcards);
        }

		[Authorize]
		[HttpGet("Decks")]
        [Route("Decks")]
        public IActionResult Decks()
        {
            List<Deck> decks = new List<Deck>();
            HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/Deck").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                decks = JsonConvert.DeserializeObject<List<Deck>>(data);
            }
            return View(decks);
        }

		[Authorize]
		[HttpGet("DeckGroups")]
        [Route("DeckGroups")]
        public IActionResult DeckGroups()
        {
            List<DeckGroup> deckgroups = new List<DeckGroup>();
            HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/DeckGroup").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroups = JsonConvert.DeserializeObject<List<DeckGroup>>(data);
            }
            return View(deckgroups);
        }

        [Authorize]
        [HttpGet("CreateFlashCard")]
        [Route("CreateFlashCard")]
        public IActionResult CreateFlashCard()
        {
            return PartialView();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewFlashCard(FlashCardViewModel flashcardViewModel)
        {

            FlashCard newFlashCard = new FlashCard()
            {
              FlashCardQuestion = flashcardViewModel.FlashCardQuestion,
                    FlashCardAnswer = flashcardViewModel.FlashCardAnswer
             };

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newFlashCard), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7025/api/FlashCard", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newFlashCard = JsonConvert.DeserializeObject<FlashCard>(apiResponse);
                    }
                }
                return View(newFlashCard);            
        }

        [Authorize]
        [HttpGet("EditFlashCard")]
        [Route("EditFlashCard")]
        public IActionResult EditFlashCard()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditFlashCard(FlashCard flashCard)
        {

            return Redirect("~/Dashboard/Index");
        }

        [Authorize]
        [HttpGet("DeleteFlashCard")]
        [Route("DeleteFlashCard")]
        public IActionResult DeleteFlashCard()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteFlashCard(FlashCard flashCard)
        {

            return Redirect("~/Dashboard/Index");
        }
    }
}
