using ApiStudyBuddy.Models;
using Newtonsoft.Json;

namespace StudyBuddyMVC.Service
{
    public class StudySessionService : IStudySessionService
    {
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client = new HttpClient();

        public StudySessionService()
        {
            _client.BaseAddress = baseAddress;
            
        }

        public List<StudySession> GetStudySessions()
        {
            List<StudySession> studySessions = new List<StudySession>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "StudySession").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                studySessions = JsonConvert.DeserializeObject<List<StudySession>>(data);
            }

            return studySessions;
        }
        public List<StudySession> GetFullStudySessions(int userId)
        {
            List<StudySession> studySessions = new List<StudySession>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "StudySession/maui/full/" + userId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                studySessions = JsonConvert.DeserializeObject<List<StudySession>>(data);
            }

            return studySessions;
        }
    }
}
