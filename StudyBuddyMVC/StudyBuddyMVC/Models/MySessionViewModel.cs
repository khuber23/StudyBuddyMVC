using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Models
{
    public class MySessionViewModel
    {
        public IEnumerable<DeckGroup> DeckGroups { get; set; }
        public IEnumerable<Deck> Decks { get; set; }
        public IEnumerable<FlashCard> FlashCards { get; set; }

        public int DeckGroupId { get; set; }
        public int DeckId { get; set;}
        public int FlashCardId { get; set; }

    }
}
