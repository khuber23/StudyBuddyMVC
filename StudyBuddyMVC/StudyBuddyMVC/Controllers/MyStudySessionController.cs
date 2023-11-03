using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudyBuddyMVC.Models;
using StudyBuddyMVC.Service;

namespace StudyBuddyMVC.Controllers
{
	public class MyStudySessionController : Controller
	{
        private readonly IUserService _userService;

		public MyStudySessionController(IUserService userService)
		{
			_userService = userService;
		}

        [Authorize]
		[HttpGet("UserDecks")]
		[Route("UserDecks")]
		public IActionResult UserDecks()
		{
            DecksViewModel vm = new DecksViewModel();
            vm.Decks = new List<SelectListItem>();

            User user = new User();
            var userid = _userService.GetUserId();

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
            vm.Decks.Add(new SelectListItem
            {
                Text = "Select a Deck",
                Value = ""
            });
            foreach (var item in user.UserDecks)
            {
                vm.Decks.Add(new SelectListItem
                {
                    Text = item.Deck.DeckName,
                    Value = Convert.ToString(item.DeckId)
                });
            }
            return View(vm);
		}

        [Authorize]
        [HttpPost]
        [Route("UserDecks")]
        public IActionResult UserDecks(DecksViewModel vm)
        {
            vm.Decks = new List<SelectListItem>();
            User user = new User();
            var userid = _userService.GetUserId();

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
            vm.Decks.Add(new SelectListItem
            {
                Text = "Select a Deck",
                Value = ""
            });
            foreach (var item in user.UserDecks)
            {
                vm.Decks.Add(new SelectListItem
                {
                    Text = item.Deck.DeckName,
                    Value = Convert.ToString(item.DeckId)
                });
            }
            ViewBag.Value = vm.DeckId;
            ViewBag.Text = vm.Decks.Where(m => m.Value == vm.DeckId.ToString()).FirstOrDefault().Text;

            return View(vm);
        }

        [Authorize]
		[HttpGet("UserDeckGroups")]
		[Route("UserDeckGroups")]
		public IActionResult UserDeckGroups()
		{
			DeckGroupsViewModel vm = new DeckGroupsViewModel();
			vm.DeckGroups = new List<SelectListItem>();

            User user = new User();
            var userid = _userService.GetUserId();

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
            vm.DeckGroups.Add(new SelectListItem
			{
				Text = "Select a Deck Group",
				Value = ""
			});
			foreach (var item in user.UserDeckGroups)
			{
				vm.DeckGroups.Add(new SelectListItem
				{
					Text = item.DeckGroup.DeckGroupName,
					Value = Convert.ToString(item.DeckGroupId)
				});
			}
            return View(vm);
		}

        [Authorize]
        [HttpPost]
        [Route("UserDeckGroups")]
        public IActionResult UserDeckGroups(DeckGroupsViewModel vm)
        {
            vm.DeckGroups = new List<SelectListItem>();

            User user = new User();
            var userid = _userService.GetUserId();

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
            vm.DeckGroups.Add(new SelectListItem
            {
                Text = "Select a Deck Group",
                Value = ""
            });
            foreach (var item in user.UserDeckGroups)
            {
                vm.DeckGroups.Add(new SelectListItem
                {
                    Text = item.DeckGroup.DeckGroupName,
                    Value = Convert.ToString(item.DeckGroupId)
                });
            }
            ViewBag.Value = vm.DeckGroupId;
            ViewBag.Text = vm.DeckGroups.Where(m => m.Value == vm.DeckGroupId.ToString()).FirstOrDefault().Text;
            return View(vm);
        }

        [Authorize]
		[HttpGet("MyStudySessionPriority")]
		[Route("MyStudySessionPriority")]
		public IActionResult MyStudySessionPriority()
		{
			return View();
		}

        [Authorize]
        [HttpPost]
        [Route("MyStudySessionPriority")]
        public IActionResult MyStudySessionPriority(StudyPriorityViewModel studyPriorityView)
        {
            return View(studyPriorityView);
        }
    }
}
