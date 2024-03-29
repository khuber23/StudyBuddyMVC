﻿using ApiStudyBuddy.Data;
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
using StudyBuddyMVC.Service;

namespace StudyBuddyMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> logger;
		private readonly HttpClient _client;
        private readonly IUserService _userService;
		public AccountController(ILogger<AccountController> logger, IUserService userService)
		{
			this.logger = logger;
			_client = new HttpClient();
            _userService = userService;
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
                StringContent content = new StringContent(JsonConvert.SerializeObject(receivedUser), Encoding.UTF8, "application/json");
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
                httpClient.BaseAddress = new Uri("https://instruct.ntc.edu/studybuddyapi/api/user/MVC/");
                var response = httpClient.GetAsync("User?username=" + username);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    string data = result.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<User>(data);
                }
                else
                {
                    return Redirect("~/login");
                }
            }

            if (user == null)
            {
                ModelState.AddModelError("Error", "Username or password is invalid.");
                return Redirect("~/login");
            }
            else
            {
                PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
                PasswordVerificationResult passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginViewModel.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("Error", "Invalid password.");
                    return Redirect("~/login");
                }
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("~/Dashboard/Index");
        }

		[HttpGet("profile")]
		public IActionResult Profile()
		{
			// Get currently logged in user ID from the auth cookie.
			int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

			// Get user.
			User u = _userService.GetUser(userId);

			return View(u);
		}

        [HttpGet("EditProfile")]
        public IActionResult EditProfile()
        {
            // Get user id.
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Get user.
            User u = _userService.GetUser(userId);

            // Populate view model.
            EditProfileViewModel vm = new EditProfileViewModel()
            {
                EmailAddress = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username
            };

            return View(vm);
        }

		[HttpPost("EditProfile")]
		public async Task<IActionResult> EditProfile(EditProfileViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}

			// Get user id.
			int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			// Get current user.
			User current = _userService.GetUser(userId);

			PasswordHasher<string> hasher = new PasswordHasher<string>();

			// Confirm password.
			if (hasher.VerifyHashedPassword(null, current.PasswordHash, vm.OldPassword) == PasswordVerificationResult.Failed)
			{
				ModelState.AddModelError("OldPassword", "Your password is incorrect.");

				return View(vm);
			}

			// Set user fields.
			current.FirstName = vm.FirstName;
			current.LastName = vm.LastName;
            current.Username = vm.Username;
            current.Email = vm.EmailAddress;

			// Check if we should be updating the password.
			if (!string.IsNullOrEmpty(vm.NewPassword))
			{
				// Hash password.
				current.PasswordHash = hasher.HashPassword(null, vm.NewPassword);
			}

			// Update.
			await _userService.UpdateUser(current);

			return RedirectToAction(nameof(Profile));
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
