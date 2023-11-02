using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace StudyBuddyMVC.Models
{
    public class StudyPriorityViewModel
    {
        public int StudyPriorityID { get; set; }

        public int FlashCardID { get; set; }
        public List<SelectListItem>? FlashCards { get; set; }

        public int DecksID { get; set; }
        public List<SelectListItem>? Decks { get; set; }

        public int DeckGroupsID {  get; set; }
        public List<SelectListItem>? DecksGroups { get; set; }
    }
}
