using NTCStudyBuddy.Data.Data;
using NTCStudyBuddy.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTCStudyBuddy.Data.Services
{
    public class DataService
    {
        private readonly DataContext _dataContext;

        public DataService(DataContext dataContext)
        {
            _dataContext= dataContext;
        }

        public List<User> GetUsers()
        {
            return _dataContext.Users.ToList();
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
