using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IFlashCardService
    {
        public List<FlashCard> GetFlashCards();

        public FlashCard GetFlashCardById(int id);

        public Task CreateFlashCard(FlashCard flashCard);

        public Task UpdateFlashCard(FlashCard flashCard);
    }
}
