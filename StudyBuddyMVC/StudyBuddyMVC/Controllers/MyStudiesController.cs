using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace StudyBuddyMVC.Controllers
{
    public class MyStudiesController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client;

        public MyStudiesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet("MyDashboard")]
        [Route("MyDashboard")]
        public IActionResult MyDashboard()
        {
            return View();
        }


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
    }
}
