using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IStudySessionService
    {
        List<StudySession> GetStudySessions();

        List<StudySession> GetFullStudySessions(int userId);
    }
}
