using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IStudySessionFlashcardService
    {
        List<StudySessionFlashCard> GetStudySessionFlashCards();

        List<StudySessionFlashCard> GetAllStudySessionFlashCards(int id);
    }
}
