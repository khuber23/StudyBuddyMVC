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
                using (var response = await _client.PostAsync(_client.BaseAddress + "DeckGroupDeck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckGroupDeck = JsonConvert.DeserializeObject<DeckGroupDeck>(responseContent);
                }
            }
        }

        public List<DeckGroupDeck> GetDeckGroupDecks()
        {
            List<DeckGroupDeck> deckgroupDeck = new List<DeckGroupDeck>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "DeckGroupDeck").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroupDeck = JsonConvert.DeserializeObject<List<DeckGroupDeck>>(data);
            }

            return deckgroupDeck;
        }

        public async Task DeleteDeckGroupDeck(DeckGroupDeck deckGroupDeck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckGroupDeck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.DeleteAsync(_client.BaseAddress + "DeckGroupDeck/" + deckGroupDeck.DeckGroupId + "/" + deckGroupDeck.DeckId))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
