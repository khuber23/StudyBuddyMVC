using ApiStudyBuddy.Models;
using Newtonsoft.Json;

namespace StudyBuddyMVC.Service
{
    public class StudySessionFlashCardService : IStudySessionFlashcardService
    {
        Uri baseAddress = new Uri("https://instruct.ntc.edu/studybuddyapi/api/");
        private readonly HttpClient _client = new HttpClient();

        public StudySessionFlashCardService()
        {
            _client.BaseAddress = baseAddress;
        }

        public List<StudySessionFlashCard> GetStudySessionFlashCards()
        {
            List<StudySessionFlashCard> studySessionFlashcards = new List<StudySessionFlashCard>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "StudySessionFlashCard").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                studySessionFlashcards = JsonConvert.DeserializeObject<List<StudySessionFlashCard>>(data);
            }

            return studySessionFlashcards;
        }

        public List<StudySessionFlashCard> GetAllStudySessionFlashCards(int userId)
        {
            List<StudySessionFlashCard> studySessionFlashcards = new List<StudySessionFlashCard>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "StudySessionFlashCard/maui/full/" + userId).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                studySessionFlashcards = JsonConvert.DeserializeObject<List<StudySessionFlashCard>>(data);
            }

            return studySessionFlashcards;
        }
    }
}
