using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IUserService
    {
        public User GetUser(int userid);
        string GetUserId();

        Task AddUserDeckGroup(UserDeckGroup deckGroup);

        Task AddUserDeck(UserDeck Userdeck);
    }
}