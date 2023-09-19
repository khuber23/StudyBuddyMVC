using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;

namespace NTCStudyBuddyMVC.Services
{
    public class DataService
    {
        private readonly ApiStudyBuddyContext _dataContext;

        public DataService(ApiStudyBuddyContext dataContext)
        {
            _dataContext = dataContext;
        }

        public User AddUser(User user)
        {
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
            return user;
        }

        public User GetUser(string emailAddress)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Email.ToLower() == emailAddress.ToLower());
        }
    }
}
