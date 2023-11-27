using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IFlashCardService
    {
        public List<FlashCard> GetFlashCards();

        public Task CreateFlashCard(FlashCard flashCard);
    }
}
