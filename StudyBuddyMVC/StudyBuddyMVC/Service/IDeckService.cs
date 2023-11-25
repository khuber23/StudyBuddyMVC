using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IDeckService
    {
        public List<Deck> GetDecks();

        public Task CreateDeck(Deck deck);
    }
}
