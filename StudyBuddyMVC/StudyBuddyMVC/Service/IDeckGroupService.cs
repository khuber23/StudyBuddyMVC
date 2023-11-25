using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IDeckGroupService
    {
        public List<DeckGroup> GetDeckGroups();

        Task CreateDeckGroup(DeckGroup deckgroup);

        DeckGroup RetrieveLastDeckGroup();
    }
}
