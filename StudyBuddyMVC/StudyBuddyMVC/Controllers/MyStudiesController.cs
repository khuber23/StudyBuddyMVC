using ApiStudyBuddy.Endpoints;
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
    [Route("MyStudies")]
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
            List<FlashCard> publicFlashcards = new List<FlashCard>();

            List<FlashCard> retrievedFlashcards = _flashCardService.GetFlashCards();
            foreach (FlashCard fc in retrievedFlashcards)
            {
                if (fc.IsPublic == true)
                {
                    publicFlashcards.Add(fc);
                }
            }
            
            return View(publicFlashcards);
        }

		[Authorize]
		[HttpGet("Decks")]
        [Route("Decks")]
        public IActionResult Decks()
        {
            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(id);

            List<Deck> decks = new List<Deck>();
            foreach (UserDeck deck in user.UserDecks)
            {
                decks.Add(deck.Deck);
            }

            return View(decks);
        }

		[Authorize]
		[HttpGet("DeckGroups")]
        [Route("DeckGroups")]
        public IActionResult DeckGroups()
        {
            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(id);

            List<DeckGroup> deckgroups = new List<DeckGroup>();
            foreach (UserDeckGroup dg in user.UserDeckGroups)
            {
                deckgroups.Add(dg.DeckGroup);
            }

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
            if (deckGroup.DeckGroupName == "test")
            {
                return RedirectToAction("DeckGroupDeck", "MyStudies");
			}

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

                    return RedirectToAction("DeckGroupDeck", "MyStudies", new { id = deckGroup1.DeckGroupId });
                }
            }

            return RedirectToAction("CreateDeckGroup", "MyStudies");
        }

        [Authorize]
        [HttpGet("DeckGroupDeck")]
        public IActionResult DeckGroupDeck(int id)
        {
            DeckGroupDeck deckgroupDeck = new DeckGroupDeck();
            deckgroupDeck.DeckGroupId = id;
            return View("DeckGroupDeck", deckgroupDeck);
        }

        [Authorize]
        [HttpPost("DeckGroupDeck")]
        public async Task<IActionResult> DeckGroupDeck(DeckGroupDeck dgb)
        {
			if (dgb.Deck.DeckName == "test")
			{
				return RedirectToAction("CreateFlashCard", "MyStudies");
			}

			Deck newDeck = new Deck();
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

                    dgb.DeckId = newDeck.DeckId;

                    // add both deck and deckgroup id to DGD.
                    await _deckGroupDeckService.CreateDeckGroupDeck(dgb);

                    // Now navigate to create flash card.
                    return RedirectToAction("CreateFlashCard", "MyStudies", new { id = dgb.DeckId });
                }
            }

            return RedirectToAction("CreateDeckGroup", "MyStudies");
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

                    return RedirectToAction("CreateFlashCard", "MyStudies", new { id = newDeck.DeckId});
                }
            }

            return RedirectToAction("Decks", "MyStudies");
        }


        [Authorize]
        [HttpGet("CreateFlashCard")]
        public IActionResult CreateFlashCard(int id)
        {
            FlashCardViewModel flashCardViewModel = new FlashCardViewModel();
            flashCardViewModel.DeckID = id;
            return View("_CreateFlashCard", flashCardViewModel);
        }

        [Authorize]
        [HttpPost("CreateFlashCard")]
        public async Task<IActionResult> CreateFlashCard(FlashCardViewModel flashCardViewModel)
        {
            DeckFlashCard deckFlashCard = new DeckFlashCard();

            // Set the fields.
            FlashCard newFlashCard = new FlashCard();
            newFlashCard.FlashCardQuestion = flashCardViewModel.FlashCardQuestion;
            newFlashCard.FlashCardAnswer = flashCardViewModel.FlashCardAnswer;


            if (!ModelState.IsValid)
            {
                return View();
            }

            // Create the flash card in order to generate an ID for the new flashcard.  
            await _flashCardService.CreateFlashCard(newFlashCard);

            // Now get the list of flashcards, which should now include the newly created flashcard.
            List<FlashCard> flashCards = _flashCardService.GetFlashCards();
            foreach (FlashCard fc in flashCards)
            {
                if (fc.FlashCardQuestion == flashCardViewModel.FlashCardQuestion && fc.FlashCardAnswer == flashCardViewModel.FlashCardAnswer)
                {
                    // Set the new flashcard again to also capture its new ID.
                    newFlashCard = fc;

                    deckFlashCard.FlashCardId = newFlashCard.FlashCardId;
                    deckFlashCard.DeckId = flashCardViewModel.DeckID;
                    await _deckService.CreateDeckFlashCard(deckFlashCard);

                    return RedirectToAction("Flashcards", "MyStudies");
                }
            }

            return RedirectToAction("ErrorReply", "MyStudies", new { id = 5 });
        }

        [Authorize]
        [HttpPost("RemoveFlashCard")]
        public IActionResult RemoveFlashCard(UserDeck userDeck)
        {
            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("AddToDeck")]
        public IActionResult AddToDeck(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 4 });
            }

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
                            return RedirectToAction("ErrorReply", "MyStudies", new { id = 2 });
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
            if (id == 0)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 3 });
            }
            Deck deck = _deckService.GetDeckByID(id);

            DeckGroupsViewModel model = new DeckGroupsViewModel();
            model.Deck = deck;
            model.DeckGroups = new List<SelectListItem>();

            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(ID);

            // Add an empty default list item.
            model.DeckGroups.Add(new SelectListItem
            {
                Text = "Select a Deck",
                Value = ""
            });

            foreach (var item in user.UserDeckGroups)
            {
                model.DeckGroups.Add(new SelectListItem
                {
                    Text = item.DeckGroup.DeckGroupName,
                    Value = Convert.ToString(item.DeckGroupId)
                });
            }

            return View(model);
        }

        [Authorize]
        [HttpPost("AddToDeckGroup")]
        public async Task<IActionResult> AddToDeckGroup(DeckGroupsViewModel deckgroupViewModel)
        {
            DeckGroupDeck deckgroupDeck = new DeckGroupDeck();
            List<DeckGroupDeck> deckgroupDecks = _deckGroupDeckService.GetDeckGroupDecks();

            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(ID);

            // Using nested loop to check and see if deck has already been assigned to deckgroup.
            foreach (UserDeckGroup userdeckGroup in user.UserDeckGroups)
            {
                if (userdeckGroup.DeckGroupId == deckgroupViewModel.DeckGroupId)
                {
                    foreach (DeckGroupDeck dgd in deckgroupDecks)
                    {
                        if (dgd.DeckGroupId == userdeckGroup.DeckGroupId && dgd.DeckId == deckgroupViewModel.Deck.DeckId)
                        {
                            return RedirectToAction("ErrorReply", "MyStudies", new { id = 1 });
                        }
                    }
                }
            }

            deckgroupDeck.DeckId = deckgroupViewModel.Deck.DeckId;
            deckgroupDeck.DeckGroupId = deckgroupViewModel.DeckGroupId;
            await _deckGroupDeckService.CreateDeckGroupDeck(deckgroupDeck);

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
        public IActionResult EditFlashCard(int id)
        {
            FlashCard flashcard = _flashCardService.GetFlashCardById(id); 
            return View(flashcard);
        }

        [Authorize]
        [HttpPost("EditFlashCard")]
        public async Task<IActionResult> EditFlashCard(FlashCard flashCard)
        {
            await _flashCardService.UpdateFlashCard(flashCard);
            return RedirectToAction("FlashCards", "MyStudies");
        }

        [Authorize]
        [HttpPost("DeleteDeckGroup")]
        public async Task<IActionResult> DeleteDeckGroupAsync(int id)
        {
            DeckGroup deckGroup = _deckGroupService.GetDeckGroupByID(id);

            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);

            UserDeckGroup userDeckGroup = new UserDeckGroup();
            userDeckGroup.UserId = ID;
            userDeckGroup.DeckGroupId = deckGroup.DeckGroupId;
            // Delete from userdeckgroup
            await _deckGroupService.DeleteUserDeckGroup(userDeckGroup);

            // Now delete the Deckgroup itself.
            await _deckGroupService.DeleteDeckGroupByID(id);

            // Problem! when trying to delete a Deckgroup, deleting userdeckgroup does not completely delete the deck itself. 
            // So had to delete the actual Deckgroup itself. But when deleting the deckgroup, it will also delete the deckgroupdeck, which is good. 

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpPost("DeleteDeck")]
        public async Task<IActionResult> DeleteDeck(int id)
        {
            Deck deck = _deckService.GetDeckByID(id);

            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);

            UserDeck userDeck = new UserDeck();
            userDeck.UserId = ID;
            userDeck.DeckId = deck.DeckId;

            await _deckService.DeleteUserDeck(userDeck);
            await _deckService.DeleteDeckID(id);

            // Same issue as deleting Deck. Deleting user deck does not delete the deck so had to delete the actual deck after. But will delete the Deckflash card. 

            return RedirectToAction("Decks", "MyStudies");
        }

        [Authorize]
        [HttpPost("DeleteFlashCard")]
        public async Task<IActionResult> DeleteFlashCard(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 0 });
            }

            await _flashCardService.DeleteFlashCardById(id);
            return RedirectToAction("Flashcards", "MyStudies");
        }

        [Authorize]
        [HttpPost("DeleteDeckGroupDeck")]
        public async Task<IActionResult> DeleteDeckGroupDeck(DeckgroupDeckFlashcardViewModel viewModel)
        {
            if (viewModel.DeckGroupId == 0 && viewModel.DeckId == 0)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 0 });
            }

            DeckGroupDeck deckGroupDeck = new DeckGroupDeck();
            deckGroupDeck.DeckId = viewModel.DeckId;
            deckGroupDeck.DeckGroupId = viewModel.DeckGroupId;

            await _deckGroupDeckService.DeleteDeckGroupDeck(deckGroupDeck);


            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("DeckGroupDetails")]
        public IActionResult DeckGroupDetails(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 0 });
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

            // Pass it into the view. May not need a post method for this.
            return View(model);
        }

        [Authorize]
        [HttpGet("DeckDetails")]
        public IActionResult DeckDetails(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 0 });
            }

            DeckgroupDeckFlashcardViewModel model = new DeckgroupDeckFlashcardViewModel();

            Deck deck = _deckService.GetDeckByID(id);

            // Get the user/owner
            var userid = _userService.GetUserId();
            int ID = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(ID);

            List<FlashCard> newFlashcardList = new List<FlashCard>();

            List<DeckFlashCard> deckFlashCards = _deckService.GetDeckFlashCards();
            foreach (DeckFlashCard df in deckFlashCards)
            {
                if (df.DeckId == id)
                {
                    newFlashcardList.Add(df.FlashCard);
                }
            }

            model.User = user;
            model.Deck = deck;
            model.FlashCards = newFlashcardList;
            model.DeckFlashCards = deckFlashCards;

            return View(model);
        }

        // ------------------------------------------ -------------- Share 
        [Authorize]
        [HttpGet("ShareDeckGroupAccess")]
        public IActionResult ShareDeckGroupAccess()
        {
            DeckGroupsViewModel vm = new DeckGroupsViewModel();
            vm.DeckGroups = new List<SelectListItem>();
            vm.Users = new List<SelectListItem>();

            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(id);

            vm.DeckGroups.Add(new SelectListItem
            {
                Text = "Select a Deck Group",
                Value = ""
            });
            foreach (var item in user.UserDeckGroups)
            {
                vm.DeckGroups.Add(new SelectListItem
                {
                    Text = item.DeckGroup.DeckGroupName,
                    Value = Convert.ToString(item.DeckGroupId)
                });
            }

            List<User> users = _userService.GetAllUsers();

            vm.Users.Add(new SelectListItem
            {
                Text = "Select a user",
                Value = "",
                //Selected = false
            });

            foreach (var item in users)
            {
                // if not equal to myself
                if (item.UserId != id)
                {
                    vm.Users.Add(new SelectListItem
                    {
                        Text = item.Username,
                        Value = Convert.ToString(item.UserId)
                    });
                }           
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost("ShareDeckGroupAccess")]
        public async Task<IActionResult> ShareDeckGroupAccessAsync(DeckGroupsViewModel viewModel)
        {
            List<UserDeckGroup> userDeckGroups = _userService.GetUserDeckGroups();

            // Get the real deckgroup to ensure it exist.
            DeckGroup deckGroup = _deckGroupService.GetDeckGroupByID(viewModel.DeckGroupId);

            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);

            foreach (UserDeckGroup userdg in userDeckGroups)
            {
                if (userdg.UserId != id)
                {
                    if (userdg.DeckGroupId == viewModel.DeckGroupId && userdg.UserId == viewModel.UserId)
                    {
                        return RedirectToAction("ErrorReply", "MyStudies", new { id = 6 });
                    }
                }   
            }

            UserDeckGroup newUserDeckGroup = new UserDeckGroup();
            newUserDeckGroup.DeckGroupId = deckGroup.DeckGroupId;
            newUserDeckGroup.UserId = viewModel.UserId;

            await _userService.AddUserDeckGroup(newUserDeckGroup);

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("ShareDeckAccess")]
        public IActionResult ShareDeckAccess()
        {
            DecksViewModel vm = new DecksViewModel();
            vm.Decks = new List<SelectListItem>();
            vm.Users = new List<SelectListItem>();

            // Get user 
            var userid = _userService.GetUserId();
            int Id = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(Id);

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

            List<User> users = _userService.GetAllUsers();

            vm.Users.Add(new SelectListItem
            {
                Text = "Select a user",
                Value = "",
                //Selected = false
            });

            foreach (var item in users)
            {
                // if not equal to myself
                if (item.UserId != Id)
                {
                    vm.Users.Add(new SelectListItem
                    {
                        Text = item.Username,
                        Value = Convert.ToString(item.UserId)
                    });
                }
            }
            return View(vm);
        }


        [Authorize]
        [HttpPost("ShareDeckAccess")]
        public async Task<IActionResult> ShareDeckAccessAsync(DecksViewModel viewModel)
        {
            List<UserDeck> userDecks = _userService.GetUserDecks();

            // Get the real deckgroup to ensure it exist.
            Deck deck = _deckService.GetDeckByID(viewModel.DeckId);

            // Get the user's own deck
            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);

            foreach (UserDeck userdg in userDecks)
            {
                if (userdg.UserId != id)
                {
                    if (userdg.DeckId == viewModel.DeckId && userdg.UserId == viewModel.UserId)
                    {
                        return RedirectToAction("ErrorReply", "MyStudies", new { id = 7 });
                    }
                }
            }

            UserDeck newUserDeck = new UserDeck();
            newUserDeck.DeckId = deck.DeckId;
            newUserDeck.UserId = viewModel.UserId;

            await _userService.AddUserDeck(newUserDeck);

            return RedirectToAction("Decks", "MyStudies");
        }

        [Authorize]
        [HttpGet("ShareDeckGroupClone")]
        public IActionResult ShareDeckGroupClone()
        {
            DeckGroupsViewModel vm = new DeckGroupsViewModel();
            vm.DeckGroups = new List<SelectListItem>();
            vm.Users = new List<SelectListItem>();

            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(id);

            vm.DeckGroups.Add(new SelectListItem
            {
                Text = "Select a Deck Group",
                Value = ""
            });
            foreach (var item in user.UserDeckGroups)
            {
                vm.DeckGroups.Add(new SelectListItem
                {
                    Text = item.DeckGroup.DeckGroupName,
                    Value = Convert.ToString(item.DeckGroupId)
                });
            }

            List<User> users = _userService.GetAllUsers();

            vm.Users.Add(new SelectListItem
            {
                Text = "Select a user",
                Value = "",
                //Selected = false
            });

            foreach (var item in users)
            {
                // if not equal to myself
                if (item.UserId != id)
                {
                    vm.Users.Add(new SelectListItem
                    {
                        Text = item.Username,
                        Value = Convert.ToString(item.UserId)
                    });
                }
            }

            return View(vm);
        }

        [Authorize]
        [HttpPost("ShareDeckGroupClone")]
        public async Task<IActionResult> ShareDeckGroupCloneAsync(DeckGroupsViewModel viewModel)
        {
            List<UserDeckGroup> userDeckGroups = _userService.GetUserDeckGroups();

            // Get the real deckgroup.
            DeckGroup deckGroup = _deckGroupService.GetDeckGroupByID(viewModel.DeckGroupId);

            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);

            // Check to make sure user isnt already assigned to that deckgroup.
            foreach (UserDeckGroup userdg in userDeckGroups)
            {
                if (userdg.UserId != id)
                {
                    if (userdg.UserId == viewModel.UserId && userdg.DeckGroup.DeckGroupName == deckGroup.DeckGroupName)
                    {
                        return RedirectToAction("ErrorReply", "MyStudies", new { id = 6 });
                    }
                }
            }  

            // Set the new deck group's fields besides Id. 
            DeckGroup newdeckGroup = new DeckGroup();
            newdeckGroup.DeckGroupName = deckGroup.DeckGroupName;
            newdeckGroup.DeckGroupDescription = deckGroup.DeckGroupDescription;
            newdeckGroup.ReadOnly = true;

            // Create the new cloned deckgroup
            await _deckGroupService.CreateDeckGroup(newdeckGroup);

            // Grab the last created deckgropu right away
            newdeckGroup = _deckGroupService.RetrieveLastDeckGroup();
            if (newdeckGroup.DeckGroupName != deckGroup.DeckGroupName && newdeckGroup.DeckGroupDescription != deckGroup.DeckGroupDescription)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 8 });
            }

            UserDeckGroup newUserDeckGroup = new UserDeckGroup();
            newUserDeckGroup.DeckGroupId = newdeckGroup.DeckGroupId;
            newUserDeckGroup.UserId = viewModel.UserId;

            await _userService.AddUserDeckGroup(newUserDeckGroup);

            return RedirectToAction("DeckGroups", "MyStudies");
        }

        [Authorize]
        [HttpGet("ShareDeckClone")]
        public IActionResult ShareDeckClone()
        {
            DecksViewModel vm = new DecksViewModel();
            vm.Decks = new List<SelectListItem>();
            vm.Users = new List<SelectListItem>();

            // Get user 
            var userid = _userService.GetUserId();
            int Id = System.Convert.ToInt32(userid);
            User user = _userService.GetUser(Id);

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

            List<User> users = _userService.GetAllUsers();

            vm.Users.Add(new SelectListItem
            {
                Text = "Select a user",
                Value = "",
                //Selected = false
            });

            foreach (var item in users)
            {
                // if not equal to myself
                if (item.UserId != Id)
                {
                    vm.Users.Add(new SelectListItem
                    {
                        Text = item.Username,
                        Value = Convert.ToString(item.UserId)
                    });
                }
            }
            return View(vm);
        }

        [Authorize]
        [HttpPost("ShareDeckClone")]
        public async Task<IActionResult> ShareDeckClone(DecksViewModel viewModel)
        {
            List<UserDeck> userDecks = _userService.GetUserDecks();

            // Get the real deckgroup.
            Deck deck = _deckService.GetDeckByID(viewModel.DeckId);

            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);

            // Check to make sure user isnt already assigned to that deck.
            foreach (UserDeck ud in userDecks)
            {
                if (ud.UserId != id)
                {
                    if (ud.UserId == viewModel.UserId && ud.Deck.DeckName == deck.DeckName)
                    {
                        return RedirectToAction("ErrorReply", "MyStudies", new { id = 7 });
                    }
                }
            }

            // Set the new deck fields besides Id. 
            Deck newDeck = new Deck();
            newDeck.DeckName = deck.DeckName;
            newDeck.DeckDescription = deck.DeckDescription;
            newDeck.ReadOnly = true;

            // Create the new cloned deck
            await _deckService.CreateDeck(newDeck);

            // Grab the last created decku right away
            newDeck = _deckService.RetrieveLastDeck();
            if (newDeck.DeckName != deck.DeckName && newDeck.DeckDescription != deck.DeckDescription)
            {
                return RedirectToAction("ErrorReply", "MyStudies", new { id = 9 });
            }

            UserDeck newUserDeck = new UserDeck();
            newUserDeck.DeckId = newDeck.DeckId;
            newUserDeck.UserId = viewModel.UserId;

            await _userService.AddUserDeck(newUserDeck);

            return RedirectToAction("Decks", "MyStudies");
        }

        [Authorize]
        [HttpGet("ErrorReply")]
        public async Task<IActionResult> ErrorReply(int id)
        {
            ErrorReplyViewModel errorReply = new ErrorReplyViewModel();
            errorReply.ErrorNumber = id;
            return View(errorReply);
        }
	}
}
