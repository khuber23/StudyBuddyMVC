using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IDeckGroupService
    {
        public List<DeckGroup> GetDeckGroups();

        public DeckGroup GetDeckGroupByID(int id);

        Task CreateDeckGroup(DeckGroup deckgroup);

        DeckGroup RetrieveLastDeckGroup();

        Task UpdateDeckGroup(DeckGroup deckgroup);
    }
}
