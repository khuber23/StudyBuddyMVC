using Microsoft.AspNetCore.Mvc;
using NTCStudyBuddyMVC.Models;
using System.Diagnostics;
using ApiStudyBuddy.Data;
using Microsoft.Extensions.Options;

namespace NTCStudyBuddyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiStudyBuddyContext _dataContext;
        private readonly IOptions<AppConfig> _appConfig;

        public HomeController(ILogger<HomeController> logger, ApiStudyBuddyContext dataContext, IOptions<AppConfig> appConfigWrapper)
        {
            _logger = logger;
            _dataContext= dataContext;
            _appConfig= appConfigWrapper;
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewBag.ApplicationName = "NTC Study Buddy";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}