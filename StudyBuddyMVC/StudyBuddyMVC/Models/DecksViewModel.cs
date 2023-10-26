using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyBuddyMVC.Models
{
    public class DecksViewModel
    {
        public int DeckId { get; set; }
        public List<SelectListItem>? Decks { get; set; }
    }
}
