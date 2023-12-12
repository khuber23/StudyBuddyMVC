using ApiStudyBuddy.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace StudyBuddyMVC.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        Uri baseAddress = new Uri("https://instruct.ntc.edu/studybuddyapi/api/");
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
                httpClient.BaseAddress = new Uri("https://instruct.ntc.edu/studybuddyapi/api/User/");
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

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "User").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<List<User>>(data);
            }
            return users;
        }

        public List<UserDeckGroup> GetUserDeckGroups()
        {
            List<UserDeckGroup> userDeckGroup = new List<UserDeckGroup>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "UserDeckGroup").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userDeckGroup = JsonConvert.DeserializeObject<List<UserDeckGroup>>(data);
            }
            return userDeckGroup;
        }

        public List<UserDeck> GetUserDecks()
        {
            List<UserDeck> userDeck = new List<UserDeck>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "UserDeck").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                userDeck = JsonConvert.DeserializeObject<List<UserDeck>>(data);
            }
            return userDeck;
        }

        public async Task DeleteUserByID(int userid)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "User/{id}?userid=" + userid).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
            }
        }

		public async Task UpdateUser(User user)
		{
			using (var httpClient = new HttpClient())
			{
				var json = JsonConvert.SerializeObject(user);
				StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
				using (var response = await _client.PutAsync(_client.BaseAddress + "User/" + user.UserId, content))

				{
					string responseContent = await response.Content.ReadAsStringAsync();
					user = JsonConvert.DeserializeObject<User>(responseContent);
				}
			}
		}
	}
}
