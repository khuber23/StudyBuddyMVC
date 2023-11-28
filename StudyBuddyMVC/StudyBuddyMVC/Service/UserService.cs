﻿using ApiStudyBuddy.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace StudyBuddyMVC.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client = new HttpClient();

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _client.BaseAddress = baseAddress;
        }

        public User GetUser(int userid)
        {
            User user = new User();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7025/api/User/");
                var response = httpClient.GetAsync("{id}?userid=" + userid);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    string data = result.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<User>(data);
                }
            }

            return user;
        }

        public async Task AddUserDeckGroup(UserDeckGroup userdeckGroup)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(userdeckGroup);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync(_client.BaseAddress + "UserDeckGroup", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    userdeckGroup = JsonConvert.DeserializeObject<UserDeckGroup>(responseContent);
                }
            }
        }

        public async Task AddUserDeck(UserDeck userDeck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(userDeck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync(_client.BaseAddress + "UserDeck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    userDeck = JsonConvert.DeserializeObject<UserDeck>(responseContent);
                }
            }
        }

        public string GetUserId()
        {
            return _contextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
