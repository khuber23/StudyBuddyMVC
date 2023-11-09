using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            List<DeckGroupViewModel> deckgroups = new List<DeckGroupViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "DeckGroup").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroups = JsonConvert.DeserializeObject<List<DeckGroupViewModel>>(data);
            }
            return View(deckgroups);
        }

        [Authorize]
        [HttpGet("CreateDeckGroup")]
        [Route("CreateDeckGroup")]
        public IActionResult CreateDeckGroup()
        {
            DeckGroupsViewModel deckgroups = new DeckGroupsViewModel();
            return PartialView("_CreateDeckGroup", deckgroups);
        }

        [Authorize]
        [HttpPost("CreateDeckGroup")]
        public async Task<IActionResult> CreateDeckGroup(DeckGroupViewModel deckGroupViewModel)
        {
            //DeckGroup deckGroup = new DeckGroup()
            //{
            //    DeckGroupName = deckGroupViewModel.DeckGroupName,
            //    DeckGroupDescription = deckGroupViewModel.DeckGroupDescription
            //};

            //using (var httpClient = new HttpClient())
            //{
            //    var json = JsonConvert.SerializeObject(deckGroup);
            //    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            //    using (var response = await _client.PostAsync("https://localhost:7025/api/DeckGroup", content))

            //    {
            //        string responseContent = await response.Content.ReadAsStringAsync();
            //        deckGroup = JsonConvert.DeserializeObject<DeckGroup>(responseContent);
            //    }
            //}
            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("CreateDeck")]
        [Route("CreateDeck")]
        public IActionResult CreateDeck()
        {
            return View();
        }

        [Authorize]
        [HttpPost("CreateDeck")]
        public async Task<IActionResult> CreateDeck(DeckGroupViewModel deckGroupViewModel)
        {
            Deck deck = new Deck()
            {
                DeckName = deckGroupViewModel.DeckGroupName,
                DeckDescription = deckGroupViewModel.DeckGroupDescription
            };

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync("https://localhost:7025/api/DeckGroup", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deck = JsonConvert.DeserializeObject<Deck>(responseContent);
                }
            }
            return RedirectToAction("MyDashboard", "MyStudies");
        }

        [Authorize]
        [HttpGet("EditDeckGroup")]
        public IActionResult EditDeckGroup(int id)
        {
            DeckGroup deckgroup;
            return PartialView();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditDeckGroup(DeckGroupViewModel deckGroupViewModel)
        {

            return Redirect("~/Dashboard/Index");
        }

        [Authorize]
        [HttpGet("CreateFlashCard")]
        [Route("CreateFlashCard")]
        public IActionResult CreateFlashCard()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateFlashCard(FlashCardViewModel flashcardViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            FlashCard flashcard = new FlashCard();
            flashcard.FlashCardQuestion = flashcardViewModel.FlashCardQuestion;
            flashcard.FlashCardAnswer = flashcardViewModel.FlashCardAnswer;

            var json = JsonConvert.SerializeObject(flashcard);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await _client.PostAsync("https://localhost:7025/api/FlashCard", content))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                flashcard = JsonConvert.DeserializeObject<FlashCard>(responseContent);
            }
            return RedirectToAction("MyDashboard", "MyStudies");

        }

        [Authorize]
        [HttpGet("EditFlashCard")]
        [Route("EditFlashCard")]
        public IActionResult EditFlashCard(int id)
        {
            //FlashCard flashCard = 
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
