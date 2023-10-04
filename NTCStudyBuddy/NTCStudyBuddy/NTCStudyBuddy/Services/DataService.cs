using NTCStudyBuddy.DataAccess.Data;

namespace NTCStudyBuddy.Services
{
    public class DataService
    {
        private readonly DataContext _dataContext;

        public DataService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
