using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using System.Data.Common;
using System.Text;

namespace StudyBuddyMVC.Controllers
{
	[Authorize]
	public class MyStudiesController : Controller
    {
         Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client;

        public DeckGroup deckGroup { get; set; }
        public List<DeckGroupViewModel> deckGroupVM { get; set; }

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

            deckGroupVM = deckgroups;

            return View(deckgroups);
        }

        [Authorize]
        [HttpGet("CreateDeckGroup")]
        public IActionResult CreateDeckGroup()
        {
            return PartialView("_CreateDeckGroup", new DeckGroupViewModel());
        }

        [Authorize]
        [HttpPost("CreateDeckGroup")]
        public async Task<IActionResult> CreateDeckGroup(DeckGroup DeckGroup)
        {
            DeckGroup deckGroup = new DeckGroup()
            {
                DeckGroupName = DeckGroup.DeckGroupName,
                DeckGroupDescription = DeckGroup.DeckGroupDescription,
                IsPublic = DeckGroup.IsPublic,
                ReadOnly = DeckGroup.ReadOnly
            };

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckGroup);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync("https://localhost:7025/api/DeckGroup", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckGroup = JsonConvert.DeserializeObject<DeckGroup>(responseContent);
                }
            }

            // call to get the deckgroups
            foreach (DeckGroupViewModel dg in deckGroupVM)
            {
                if (deckGroup.DeckGroupName == DeckGroup.DeckGroupName && deckGroup.DeckGroupDescription == DeckGroup.DeckGroupDescription)
                {
                    DeckGroup = deckGroup;
                    break;
                }
            }

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("DeckGroupDeck")]
        public IActionResult DeckGroupDeck()
        {
            return PartialView("DeckGroupDeck", new DeckGroupDeck());
        }

        [Authorize]
        [HttpPost("DeckGroupDeck")]
        public async Task<IActionResult> DeckGroupDeck(DeckGroupDeck dgb)
        {
            //DeckGroupDeck deckGroupDeckTest = new DeckGroupDeck();

            DeckGroup deckGroup = new DeckGroup();
            deckGroup.DeckGroupName = dgb.DeckGroup.DeckGroupName; ;
            deckGroup.DeckGroupDescription = dgb.DeckGroup.DeckGroupDescription;
            deckGroup.IsPublic = dgb.DeckGroup.IsPublic;
            deckGroup.ReadOnly = dgb.DeckGroup.ReadOnly;

            Deck deck = new Deck();
            deck.DeckName = "Deck Example";
            deck.DeckDescription = "Example of deck description";

            //deckGroupDeckTest.DeckGroup = deckGroup;
            //deckGroupDeckTest.Deck = deck;
            //if (dgb.Deck == null)
            //{
            //    deckGroup.IsPublic = false;
            //    return RedirectToAction("DeckGroupDeck", "MyStudies");
            //}
            //else
            //{
            //    using (var httpClient = new HttpClient())
            //    {
            //        var json = JsonConvert.SerializeObject(deckgroupDeck.Deck);
            //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            //        using (var response = await _client.PostAsync("https://localhost:7025/api/Deck", content))

            //        {
            //            string responseContent = await response.Content.ReadAsStringAsync();
            //            deckgroupDeck.Deck = JsonConvert.DeserializeObject<Deck>(responseContent);
            //        }
            //    }
            //}

            //using (var httpClient = new HttpClient())
            //{
            //    var json = JsonConvert.SerializeObject(deckgroupDeck);
            //    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            //    using (var response = await _client.PostAsync("https://localhost:7025/api/DeckGroupDeck", content))

            //    {
            //        string responseContent = await response.Content.ReadAsStringAsync();
            //        deckgroupDeck = JsonConvert.DeserializeObject<DeckGroupDeck>(responseContent);
            //    }
            //}

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("CreateDeck")]
        [Route("CreateDeck")]
        public IActionResult CreateDeck()
        {
            return PartialView("_CreateDeck", new Deck());
        }

        [Authorize]
        [HttpPost("CreateDeck")]
        public async Task<IActionResult> CreateDeck(Deck deck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync("https://localhost:7025/api/Deck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deck = JsonConvert.DeserializeObject<Deck>(responseContent);
                }
            }

            // Assigning the deckgroupdeck to both deckgroup and decks. 
            DeckGroupDeck deckgroupDeck = new DeckGroupDeck();
            deckgroupDeck.Deck.DeckName = deck.DeckName;
            deckgroupDeck.Deck.DeckDescription = deck.DeckDescription;
            deckgroupDeck.Deck.IsPublic = deck.IsPublic;
            deckgroupDeck.Deck.ReadOnly = deck.ReadOnly;

            deckgroupDeck.DeckGroup.DeckGroupName = deckGroup.DeckGroupName;
            deckgroupDeck.DeckGroup.DeckGroupDescription = deckGroup.DeckGroupDescription;
            deckgroupDeck.DeckGroup.IsPublic = deckGroup.IsPublic;
            deckgroupDeck.DeckGroup.ReadOnly = deckGroup.ReadOnly;

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckgroupDeck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync("https://localhost:7025/api/DeckGroupDeck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckgroupDeck = JsonConvert.DeserializeObject<DeckGroupDeck>(responseContent);
                }
            }

            return RedirectToAction("CreateFlashCard", "MyStudies");
        }


        [Authorize]
        [HttpGet("CreateFlashCard")]
        [Route("CreateFlashCard")]
        public IActionResult CreateFlashCard()
        {
            return PartialView("_CreateFlashCard", new FlashCard());
        }

        [Authorize]
        [HttpPost("CreateFlashCard")]
        public async Task<IActionResult> CreateFlashCard(FlashCard flashCard)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var json = JsonConvert.SerializeObject(flashCard);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await _client.PostAsync("https://localhost:7025/api/FlashCard", content))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                flashCard = JsonConvert.DeserializeObject<FlashCard>(responseContent);
            }
            return RedirectToAction("Flashcards", "MyStudies");
        }

        [Authorize]
        [HttpPost("EditDeckGroup")]
        public IActionResult EditDeckGroup(int id)
        {
            DeckGroup deckgroup;
            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpPost("EditDeck")]
        public IActionResult EditDeck(int id)
        {
            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpPost("AddToDeck")]
        public IActionResult AddToDeck(int id)
        {
            return RedirectToAction("FlashCards", "MyStudies");
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
        [HttpPost("DeleteDeckGroup")]
        public IActionResult DeleteDeckGroup(int id)
        {
            return RedirectToAction("DeckGroups", "MyStudies");
        }


        [Authorize]
        [HttpPost("DeleteDeck")]
        public IActionResult DeleteDeck(int id)
        {
            return RedirectToAction("Decks", "MyStudies");
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
