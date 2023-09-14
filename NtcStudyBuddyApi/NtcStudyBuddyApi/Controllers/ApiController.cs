using NtcStudyBuddy.DataAccess.Data;
using NtcStudyBuddy.DataAccess.Services;
using NtcStudyBuddy.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace NtcStudyBuddyApi.Controllers
{
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly DataService _dataService;

        public ApiController(DataContext dataContext)
        {
            DataService dataService = new DataService(dataContext);
            _dataService = dataService;
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            ApiResponse apiResponse = new ApiResponse();

            List<User> users = new List<User>();

            users = _dataService.GetUsers();

            // Set the api response to be a success status, and the payload to be the retrieved categories.
            apiResponse.Status = 0;
            apiResponse.Payload = users;

            string jsonData = JsonSerializer.Serialize(apiResponse);
            return Content(jsonData);
        }
    }
}
