using ApiStudyBuddy.Models;
using Newtonsoft.Json;
using System.Text;

namespace StudyBuddyMVC.Service
{
    public class DeckGroupDeckService : IDeckGroupDeckService
    {
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client = new HttpClient();

        public DeckGroupDeckService()
        {
            _client.BaseAddress = baseAddress;
        }
        public async Task CreateDeckGroupDeck(DeckGroupDeck deckGroupDeck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckGroupDeck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync("https://localhost:7025/api/DeckGroupDeck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckGroupDeck = JsonConvert.DeserializeObject<DeckGroupDeck>(responseContent);
                }
            }
        }
    }
}
