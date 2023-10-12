using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net.Mail;

namespace StudyBuddyMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;

        public AccountController(ILogger<AccountController> logger)
        {
            this.logger = logger;
        }

        // GET: Register
        [AllowAnonymous]
        [HttpGet("register")]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/AddUser
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            User receivedUser = new User();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://instruct.ntc.edu/studybuddyapi/api/user", content))
                    // https://localhost:7025/api/User
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(receivedUser);
        }

        // GET: Login
        [AllowAnonymous]
        [HttpGet("login")]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }


        public async Task<IActionResult> LoginUser(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://instruct.ntc.edu/studybuddyapi/api/token", content))
                // https://localhost:7025/api/Token
                {
                    string token = await response.Content.ReadAsStringAsync();
                    if (token == "Invalid credentials")
                    {
                        ViewBag.Message = "Incorrect Username or Password.";
                        return Redirect("~/login");
                    }
                    HttpContext.Session.SetString("JwToken", token);
                }

                return Redirect("~/Dashboard/Index");
            }
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Redirect("~/login");
        }


    }   
}
