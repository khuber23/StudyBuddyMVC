using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyBuddyMVC.Service;

namespace StudyBuddyMVC.Controllers
{
    [Authorize]
    public class MyDashboardController : Controller
    {
        private readonly IUserService _userService;
        private IStudySessionService _studySessionService;
        private IStudySessionFlashcardService _studySessionFlashcardService;
        private IDeckService _deckService;
        private IFlashCardService _flashCardSerice;
        public MyDashboardController(IUserService userService, IStudySessionFlashcardService studySessionFlashcardService, IStudySessionService studySessionService, IDeckService deckService, IFlashCardService flashCardSerice)
        {
            _userService = userService;
            _studySessionFlashcardService = studySessionFlashcardService;
            _studySessionService = studySessionService;
            _deckService = deckService;
            _flashCardSerice = flashCardSerice;
        }

        [Authorize]
        [HttpGet("Stats")]
        [Route("Stats")]
        public IActionResult MyStats()
        {
            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);
            //User user = _userService.GetUser(id);

            List<StudySessionFlashCard> studySessionsFlashcards = _studySessionFlashcardService.GetAllStudySessionFlashCards(id);

            return View(studySessionsFlashcards);
        }

		[Authorize]
		[HttpGet("MyHistory")]
        [Route("MyHistory")]
        public IActionResult MyHistory()
        {
            var userid = _userService.GetUserId();
            int id = System.Convert.ToInt32(userid);
            //User user = _userService.GetUser(id);

            List<StudySessionFlashCard> studySessionsFlashcards = _studySessionFlashcardService.GetAllStudySessionFlashCards(id);
            return View(studySessionsFlashcards);
        }
    }
}
