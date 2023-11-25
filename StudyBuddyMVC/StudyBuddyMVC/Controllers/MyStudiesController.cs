using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using StudyBuddyMVC.Service;
using System.Data.Common;
using System.Text;

namespace StudyBuddyMVC.Controllers
{
	[Authorize]
	public class MyStudiesController : Controller
    {
        // Service fields
        private readonly IUserService _userService;
        private readonly IDeckGroupService _deckGroupService;
        private readonly IDeckService _deckService;

        // Client and base address set up.
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client;

        public MyStudiesController(IDeckGroupService deckGroupService, IUserService userService, IDeckService deckService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _deckGroupService = deckGroupService;
            _userService = userService;
            _deckService = deckService;
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
            List<Deck> decks = _deckService.GetDecks();

            return View(decks);
        }

		[Authorize]
		[HttpGet("DeckGroups")]
        [Route("DeckGroups")]
        public IActionResult DeckGroups()
        {
            List<DeckGroup> deckgroups = _deckGroupService.GetDeckGroups();

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
        public async Task<IActionResult> CreateDeckGroup(DeckGroup deckGroup)
        {
            var userId = _userService.GetUserId();

            if (!ModelState.IsValid)
            {
                return View("DeckGroups", "MyStudies");
            }
   
            UserDeckGroup userDeckGroup = new UserDeckGroup();
            userDeckGroup.UserId = Int32.Parse(userId);
            userDeckGroup.DeckGroup = deckGroup;
            await _userService.AddUserDeckGroup(userDeckGroup);

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
            // This set up is not official yet, gonna try to figure this one yet to make sure we capture the right deck group first.
            //DeckGroupDeck deckGroupDeck = new DeckGroupDeck();

            //DeckGroup tempDeckGroup = _deckGroupService.RetrieveLastDeckGroup();
            //if (tempDeckGroup != null)
            //{
            //    deckGroupDeck.DeckGroup = tempDeckGroup;
            //    deckGroupDeck.Deck = deck;
            //}

            //await _deckService.CreateDeck(deck);

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
            var userId = _userService.GetUserId();

            if (!ModelState.IsValid)
            {
                return View("Deck", "MyStudies");
            }

            UserDeck userDeck = new UserDeck();
            userDeck.UserId = Int32.Parse(userId);
            userDeck.Deck = deck;
            await _userService.AddUserDeck(userDeck);

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
