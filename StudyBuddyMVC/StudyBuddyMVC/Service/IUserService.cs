using ApiStudyBuddy.Models;

namespace StudyBuddyMVC.Service
{
    public interface IUserService
    {
        public User GetUser(int userid);
        string GetUserId();

        List<User> GetAllUsers();

        List<UserDeckGroup> GetUserDeckGroups();

        List<UserDeck> GetUserDecks();

        Task AddUserDeckGroup(UserDeckGroup deckGroup);

        Task AddUserDeck(UserDeck Userdeck);
    }
}