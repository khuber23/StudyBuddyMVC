using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyBuddyMVC.Models
{
    public class DeckGroupsViewModel
    {
        public int DeckGroupId { get; set; }

        public List<SelectListItem>? DeckGroups { get; set; }

        public Deck? Deck { get; set; }

    }
}
