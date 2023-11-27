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
        private readonly IDeckGroupDeckService _deckGroupDeckService;
        private readonly IDeckService _deckService;

        // Client and base address set up.
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client;

        public MyStudiesController(IDeckGroupService deckGroupService, IDeckGroupDeckService deckGroupDeckService, IUserService userService, IDeckService deckService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _deckGroupService = deckGroupService;
            _deckGroupDeckService = deckGroupDeckService;
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
            DeckGroup deckGroup1 = new DeckGroup();
            var userId = _userService.GetUserId();

            if (!ModelState.IsValid)
            {
                return View("DeckGroups", "MyStudies");
            }

            await _deckGroupService.CreateDeckGroup(deckGroup);

            List<DeckGroup> deckgroupList = _deckGroupService.GetDeckGroups();
            foreach (DeckGroup dg in deckgroupList)
            {
                if (dg.DeckGroupName == deckGroup.DeckGroupName && dg.DeckGroupDescription == deckGroup.DeckGroupDescription)
                {
                    deckGroup1 = dg;
                }
            }

            UserDeckGroup userDeckGroup = new UserDeckGroup();
            userDeckGroup.UserId = Int32.Parse(userId);
            userDeckGroup.DeckGroupId = deckGroup1.DeckGroupId;
            await _userService.AddUserDeckGroup(userDeckGroup);

            return RedirectToAction("DeckGroupDeck", "MyStudies");
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
            Deck newDeck = new Deck();
            DeckGroup tempDeckGroup = new DeckGroup();
            DeckGroupDeck newDeckGroupDeck = new DeckGroupDeck();
            var userId = _userService.GetUserId();

            await _deckService.CreateDeck(dgb.Deck);

            // using this logic to add deck to user deck and to deckgroupdeck.
            List<Deck> decks = _deckService.GetDecks();
            foreach (Deck d in decks)
            {
                if (d.DeckName == dgb.Deck.DeckName && d.DeckDescription == dgb.Deck.DeckDescription)
                {
                    newDeck = d;

                    // clear deck from deckgroupdeck so we do not instantiate deck again again again.
                    dgb.Deck = null;

                    //Capture the user deck
                    UserDeck userDeck = new UserDeck();
                    userDeck.UserId = Int32.Parse(userId);
                    userDeck.DeckId = newDeck.DeckId;
                    await _userService.AddUserDeck(userDeck);

                    // Get the newly created deckgroup.
                    tempDeckGroup = _deckGroupService.RetrieveLastDeckGroup();
                    if (tempDeckGroup != null)
                    {
                        dgb.DeckId = newDeck.DeckId;
                        dgb.DeckGroupId = tempDeckGroup.DeckGroupId;

                        // add both deck and deckgroup id to DGD.
                        await _deckGroupDeckService.CreateDeckGroupDeck(dgb);
                    }
                }
            }

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
