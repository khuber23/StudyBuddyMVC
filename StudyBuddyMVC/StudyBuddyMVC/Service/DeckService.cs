using ApiStudyBuddy.Models;
using Newtonsoft.Json;
using System.Text;

namespace StudyBuddyMVC.Service
{
    public class DeckService : IDeckService
    {
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client = new HttpClient();

        public DeckService()
        {
            _client.BaseAddress = baseAddress;
        }
        public List<Deck> GetDecks()
        {
            List<Deck> decks = new List<Deck>();
            HttpResponseMessage response = _client.GetAsync("https://localhost:7025/api/Deck").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                decks = JsonConvert.DeserializeObject<List<Deck>>(data);
            }
            return decks;
        }
        public async Task CreateDeck(Deck deck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync("https://localhost:7025/api/Deck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deck = JsonConvert.DeserializeObject<Deck>(responseContent);
                }
            }
        }
    }
}
