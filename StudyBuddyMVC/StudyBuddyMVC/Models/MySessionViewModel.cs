using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Models
{
    public class MySessionViewModel
    {
        public int Id { get; set; }

        public User? User { get; set; }

        public DeckGroup? DeckGroup { get; set; }

        public List<UserDeckGroup>? UserDeckGroups { get; set; }

        public List<UserDeck>? UserDecks { get; set; }


    }
}
