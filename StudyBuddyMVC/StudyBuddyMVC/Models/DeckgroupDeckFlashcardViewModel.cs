using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StudyBuddyMVC.Models
{
    public class DeckgroupDeckFlashcardViewModel
    {
        public User? User { get; set; }
        public DeckGroup DeckGroup { get; set; }

        public List<Deck>? Decks { get; set; }

        public List<FlashCard>? FlashCards { get; set; }

        public List<DeckFlashCard> DeckFlashCards { get; set; }
    }
}
