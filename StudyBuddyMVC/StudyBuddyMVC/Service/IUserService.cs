using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IUserService
    {
        string GetUserId();

        Task AddUserDeckGroup(UserDeckGroup deckGroup);

        Task AddUserDeck(UserDeck Userdeck);
    }
}