using Microsoft.EntityFrameworkCore;
using NtcStudyBuddy.DataAccess.Data;
using NtcStudyBuddy.DataAccess.Models;

namespace NtcStudyBuddy.DataAccess.Services
{
    public class DataService
    {
        private readonly DataContext _dataContext;

        public DataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<User> GetUsers()
        {
            return _dataContext.Users
                .Include(u => u.UserDecks)
                .Include(u=> u.UserDeckGroups)
                .Include(u => u.StudySessions)
                .ToList();
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
