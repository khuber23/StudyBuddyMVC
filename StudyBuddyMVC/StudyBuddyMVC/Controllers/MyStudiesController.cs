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
        private readonly IFlashCardService _flashCardService;

        // Client and base address set up.
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client;

        public MyStudiesController(IDeckGroupService deckGroupService, IDeckGroupDeckService deckGroupDeckService, IUserService userService, IDeckService deckService, IFlashCardService flashCardService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
            _deckGroupService = deckGroupService;
            _deckGroupDeckService = deckGroupDeckService;
            _userService = userService;
            _deckService = deckService;
            _flashCardService = flashCardService;
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
            List<FlashCard> publicFlashcards = _flashCardService.GetFlashCards();
            
            return View(publicFlashcards);
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
            UserDeckGroup userDeckGroup = new UserDeckGroup();
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

                    userDeckGroup.UserId = Int32.Parse(userId);
                    userDeckGroup.DeckGroupId = deckGroup1.DeckGroupId;
                    await _userService.AddUserDeckGroup(userDeckGroup);

                    return RedirectToAction("DeckGroupDeck", "MyStudies");
                }
            }

            return RedirectToAction("CreateDeckGroup", "MyStudies");
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
            var userId = _userService.GetUserId();

            //Create the deck.
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

                        // Now navigate to create flash card.
                        return RedirectToAction("CreateFlashCard", "MyStudies");
                    }
                }
            }

            return RedirectToAction("CreateDeckGroup", "MyStudies");
        }

        [Authorize]
        [HttpGet("CreateDeckFromDeckGroup")]
        public IActionResult CreateDeckFromDeckGroup(int id)
        {
            DeckGroup deckGroup = _deckGroupService.GetDeckGroupByID(id);

            DeckGroupDeck deckGroupDeck = new DeckGroupDeck();
            deckGroupDeck.DeckGroup = deckGroup;

            return View(deckGroupDeck);
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
            Deck newDeck = new Deck();
            UserDeck userDeck = new UserDeck();
            var userId = _userService.GetUserId();

            if (!ModelState.IsValid)
            {
                return View();
            }

            //Create the new deck.
            await _deckService.CreateDeck(deck);

            // Now get the list of decks which should include the new deck.
            List<Deck> decks = _deckService.GetDecks();
            foreach (Deck d in decks)
            {
                if (d.DeckName == deck.DeckName && d.DeckDescription == deck.DeckDescription)
                {
                    newDeck = d;

                    // Create the userdeck. 
                    userDeck.UserId = Int32.Parse(userId);
                    userDeck.DeckId = newDeck.DeckId;
                    await _userService.AddUserDeck(userDeck);

                    return RedirectToAction("CreateFlashCard", "MyStudies");
                }
            }

            return RedirectToAction("Decks", "MyStudies");
        }


        [Authorize]
        [HttpGet("CreateFlashCard")]
        public IActionResult CreateFlashCard()
        {
            return PartialView("_CreateFlashCard", new FlashCard());
        }

        [Authorize]
        [HttpPost("CreateFlashCard")]
        public async Task<IActionResult> CreateFlashCard(FlashCard flashCard)
        {
            DeckFlashCard deckFlashCard = new DeckFlashCard();
            FlashCard newFlashCard = new FlashCard();
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Get the deck that was just created.
            Deck retrivedDeck = _deckService.RetrieveLastDeck();

            // Create the flash card in order to generate an ID for the new flashcard.  
            await _flashCardService.CreateFlashCard(flashCard);

            // Now get the list of flashcards, which should now include the newly created flashcard.
            List<FlashCard> flashCards = _flashCardService.GetFlashCards();
            foreach (FlashCard fc in flashCards)
            {
                if (fc.FlashCardQuestion == flashCard.FlashCardQuestion && fc.FlashCardAnswer == flashCard.FlashCardAnswer)
                {
                    newFlashCard = fc;

                    deckFlashCard.FlashCardId = newFlashCard.FlashCardId;
                    deckFlashCard.DeckId = retrivedDeck.DeckId;
                    await _deckService.CreateDeckFlashCard(deckFlashCard);
                }
            }

            return RedirectToAction("Flashcards", "MyStudies");
        }

        [Authorize]
        [HttpGet("AddToDeck")]
        public IActionResult AddToDeck(int id)
        {
            //DeckFlashCard deckFlashCard = new DeckFlashCard();
            FlashCard flashCard = _flashCardService.GetFlashCardById(id);

            DecksViewModel vm = new DecksViewModel();
            vm.FlashCard = flashCard;
            vm.Decks = new List<SelectListItem>();

            // Get user 
            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(ID);

            // Add an empty default list item.
            vm.Decks.Add(new SelectListItem
            {
                Text = "Select a Deck",
                Value = ""
            });
            foreach (var item in user.UserDecks)
            {
                vm.Decks.Add(new SelectListItem
                {
                    Text = item.Deck.DeckName,
                    Value = Convert.ToString(item.DeckId)
                });
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost("AddToDeck")]
        public async Task<IActionResult> AddToDeck(DecksViewModel decksViewModel)
        {
            DeckFlashCard deckFlashCard = new DeckFlashCard();
            List<DeckFlashCard> deckFlashCards = _deckService.GetDeckFlashCards();

            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(ID);

            // Using nested loop to check and see if flashcard has already been assigned to the user's own deck. 
            foreach (UserDeck userDeck in user.UserDecks)
            {
                if (userDeck.DeckId == decksViewModel.DeckId)
                {
                    foreach (DeckFlashCard df in deckFlashCards)
                    {
                        if (df.DeckId == userDeck.DeckId && df.FlashCardId == decksViewModel.FlashCard.FlashCardId)
                        {
                            return new ContentResult { Content = "Yikes! Looks like this flashcard has already been assigned to this Deck." };
                        }
                    }
                }
            }

            // Proceed to create a new deck flashcard. 
            deckFlashCard.DeckId = decksViewModel.DeckId;
            deckFlashCard.FlashCardId = decksViewModel.FlashCard.FlashCardId;
            await _deckService.CreateDeckFlashCard(deckFlashCard); 

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("AddToDeckGroup")]
        public IActionResult AddToDeckGroup(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost("AddToDeckGroup")]
        public IActionResult AddToDeckGroup(DeckGroupsViewModel deckgroupViewModel)
        {
            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("EditDeckGroup")]
        public IActionResult EditDeckGroup(int id)
        {
            DeckGroup deckgroup = _deckGroupService.GetDeckGroupByID(id);
            return View(deckgroup);
        }

        [Authorize]
        [HttpPost("EditDeckGroup")]
        public async Task<IActionResult> EditDeckGroup(DeckGroup deckGroup)
        {
            await _deckGroupService.UpdateDeckGroup(deckGroup);

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("EditDeck")]
        public IActionResult EditDeck(int id)
        {
            Deck deck = _deckService.GetDeckByID(id);
            return View(deck);
        }

        [Authorize]
        [HttpPost("EditDeck")]
        public async Task<IActionResult> EditDeck(Deck deck)
        {
            await _deckService.UpdateDeck(deck);
            return RedirectToAction("Decks", "MyStudies");
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

        [Authorize]
        [HttpGet("DeckGroupDetails")]
        public IActionResult DeckGroupDetails(int id)
        {
            if (id == 0)
            {
                return new ContentResult { Content = "Bummers! That Deckgroup ID does not exist. Go back and try again." };
            }
            DeckgroupDeckFlashcardViewModel model = new DeckgroupDeckFlashcardViewModel();

            // Find the deckgroup.
            DeckGroup deckGroup = _deckGroupService.GetDeckGroupByID(id);

            // Get the user/owner
            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(ID);

            // Create lists to collect the values.
            List<Deck> newDeckList = new List<Deck>();
            List<FlashCard> newFlashcardList = new List<FlashCard>();
            
            // Loop through to find decks that were assigned to the Deckgroup.
            List<DeckGroupDeck> deckGroupDecks = _deckGroupDeckService.GetDeckGroupDecks();
            foreach (DeckGroupDeck dgd in deckGroupDecks)
            {
                if (dgd.DeckGroupId == id)
                {
                    newDeckList.Add(dgd.Deck);
                }
            }

            // Nest loop to get the assigned flashcard from the deck(s).
            List<DeckFlashCard> deckflashCards = _deckService.GetDeckFlashCards();
            foreach (DeckFlashCard df in deckflashCards)
            {
                foreach (Deck d in newDeckList) 
                {
                    if (d.DeckId == df.DeckId)
                    {
                        newFlashcardList.Add(df.FlashCard);
                    }
                }
            }

            // Assigning all values to the models properties.
            model.User = user;
            model.DeckGroup = deckGroup;
            model.Decks = newDeckList;
            model.FlashCards = newFlashcardList;
            model.DeckFlashCards = deckflashCards;

            return View(model);
        }
    }
}
