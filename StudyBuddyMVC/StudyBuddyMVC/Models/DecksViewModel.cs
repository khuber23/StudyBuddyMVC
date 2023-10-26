using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyBuddyMVC.Models
{
    public class DecksViewModel
    {
        public IEnumerable<SelectListItem>? Decks { get; set; }
        public IEnumerable<string>? SelectedDecks { get; set; }
    }
}
