using Microsoft.AspNetCore.Mvc;

namespace StudyBuddyMVC.Controllers
{
    public class MyStudiesController : Controller
    {
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
            return View();
        }

        [HttpGet("Decks")]
        [Route("Decks")]
        public IActionResult Decks()
        {
            return View();
        }

        [HttpGet("DeckGroups")]
        [Route("DeckGroups")]
        public IActionResult DeckGroups()
        {
            return View();
        }
    }
}
