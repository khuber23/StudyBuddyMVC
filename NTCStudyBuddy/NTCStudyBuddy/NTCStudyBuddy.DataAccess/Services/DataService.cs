using NTCStudyBuddy.DataAccess.Data;
using NTCStudyBuddy.DataAccess.Models;

namespace NTCStudyBuddy.DataAccess.Services
{
    public class DataService
    {
        private readonly DataContext _dataContext;

        public DataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User AddUser(User user)
        {
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
            return user;
        }

        // Update User

        // Get User

        // Get UserDeck

        // ====================================================

        // Add FlashCard 

        // Get FlashCards

        // Get DeckFlashCards

        //======================================================

        // Add Deck

        // Get Decks

        // Add DeckGroup

        // Get DeckGroups
    }
}
