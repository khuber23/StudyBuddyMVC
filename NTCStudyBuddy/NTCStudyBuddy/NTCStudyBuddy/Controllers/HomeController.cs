using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NTCStudyBuddy.DataAccess.Data;
using NTCStudyBuddy.DataAccess.Services;

namespace NTCStudyBuddy.Controllers
{
    [ApiController]
    [Route("Home")]
    public class HomeController : ControllerBase
    {
        private readonly AppConfig _appConfig;

        private readonly ILogger<HomeController> _logger;

        private readonly DataService _dataService;

        public HomeController(ILogger<HomeController> logger, IOptions<AppConfig> appConfigWrapper, DataContext dataContext)
        {
            _logger = logger;
            _appConfig = appConfigWrapper.Value;
            _dataService = new DataService(dataContext);
        }
    };

}