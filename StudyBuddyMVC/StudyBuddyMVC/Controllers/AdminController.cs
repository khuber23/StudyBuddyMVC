using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddyMVC.Service;

namespace StudyBuddyMVC.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IUserService _userService;
        private readonly IDeckGroupService _deckGroupService;
        private readonly IDeckGroupDeckService _deckGroupDeckService;
        private readonly IDeckService _deckService;
        private readonly IFlashCardService _flashCardService;

        public AdminController(ILogger<AdminController> logger, IUserService userService, IDeckGroupService deckGroupService, IDeckGroupDeckService deckGroupDeckService, IDeckService deckService, IFlashCardService flashCardService)
        {
            _logger = logger;
            _userService = userService;
            _deckGroupService = deckGroupService;
            _deckGroupDeckService = deckGroupDeckService;
            _deckService = deckService;
            _flashCardService = flashCardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Flashcards")]
        [Route("Flashcards")]
        public IActionResult Flashcards()
        {
            List<FlashCard> retrievedFlashcards = _flashCardService.GetFlashCards();
            return View(retrievedFlashcards);
        }

        [HttpGet("Decks")]
        [Route("Decks")]
        public IActionResult Decks()
        {
            List<Deck> decks = _deckService.GetDecks();
            return View(decks);
        }

        [HttpGet("DeckGroups")]
        [Route("DeckGroups")]
        public IActionResult DeckGroups()
        {
            List<DeckGroup> deckgroups = _deckGroupService.GetDeckGroups();
            return View(deckgroups);
        }

        [HttpGet("Users")]
        [Route("Users")]
        public IActionResult Users()
        {
            List<User> users = _userService.GetAllUsers();
            return View(users);
        }


        [Authorize]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserByID(id);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
