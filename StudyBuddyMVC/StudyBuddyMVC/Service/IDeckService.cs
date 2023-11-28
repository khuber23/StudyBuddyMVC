using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IDeckService
    {
        public List<Deck> GetDecks();

        public Deck GetDeckByID(int id);

        public List<DeckFlashCard> GetDeckFlashCards();

        public Task CreateDeck(Deck deck);

        public Task CreateDeckFlashCard(DeckFlashCard deckFlashCard);

        public Task UpdateDeck(Deck deck);

        Deck RetrieveLastDeck();
    }
}
