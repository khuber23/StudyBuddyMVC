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
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
		private readonly HttpClient _client;
		public AccountController(ILogger<AccountController> logger)
		{
			this.logger = logger;
			_client = new HttpClient();
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
		[AllowAnonymous]
		[HttpPost]
        public async Task<IActionResult> AddUser(RegisterViewModel registerViewModel)
        {
            // Need to check existing user yet

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            User receivedUser = new User()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email,
                Username = registerViewModel.Username,
                PasswordHash = passwordHasher.HashPassword(null, registerViewModel.Password)
            };
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

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> LoginUser(LoginViewModel loginViewModel, string username)
        {

            if (!ModelState.IsValid)
            {
				return Redirect("~/login");
			}

			User user = new User()
            {
				Username = loginViewModel.Username
			};

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

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties {};
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

			return Redirect("~/Dashboard/Index");
		}

        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/login");
        }
    }   
}
