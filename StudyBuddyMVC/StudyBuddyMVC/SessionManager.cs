using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace StudyBuddyMVC
{
    public class SessionManager
    {
        static SessionManager instance;
        public StudySession session;
        public StudySessionFlashCard studySessionFlashCard;
        public FlashCard flashCard;
		private ApiStudyBuddyContext context;
        public bool IsComplete = false;
        public List<StudySessionFlashCard> incorrectFlashCard = new List<StudySessionFlashCard>();
		public List<StudySessionFlashCard> correctFlashCard = new List<StudySessionFlashCard>();

		public SessionManager()
        {         
            session = new StudySession();
            session.StartTime = DateTime.Now;
        }

        public static SessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionManager();
                }
                return instance;
            }
        }

        public FlashCard LoadSession()
        {
            var flashcard = context.FlashCards.Find(flashCard);
            return flashcard;
        }

        public void InCorrectFlashCard()
        {
            if (studySessionFlashCard.IsCorrect == false)
            {
				incorrectFlashCard.Add(studySessionFlashCard);
			}
            
        }

		public void CorrectFlashCard()
		{
			if (studySessionFlashCard.IsCorrect == true)
			{
				correctFlashCard.Add(studySessionFlashCard);
			}

		}
	}
}
