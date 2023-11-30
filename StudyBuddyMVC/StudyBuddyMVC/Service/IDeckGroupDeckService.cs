using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IDeckGroupDeckService
    {
        Task CreateDeckGroupDeck(DeckGroupDeck deckGroupDeck);

        List<DeckGroupDeck> GetDeckGroupDecks();

        Task DeleteDeckGroupDeck(DeckGroupDeck deckGroupDeck);
    }
}
