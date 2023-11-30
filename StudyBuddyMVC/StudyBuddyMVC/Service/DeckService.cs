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
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Deck").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                decks = JsonConvert.DeserializeObject<List<Deck>>(data);
            }
            return decks;
        }

        public List<DeckFlashCard> GetDeckFlashCards()
        {
            List<DeckFlashCard> deckFlashCard = new List<DeckFlashCard>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "DeckFlashCard").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckFlashCard = JsonConvert.DeserializeObject<List<DeckFlashCard>>(data);
            }
            return deckFlashCard;
        }

        public async Task CreateDeck(Deck deck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync(_client.BaseAddress + "Deck", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deck = JsonConvert.DeserializeObject<Deck>(responseContent);
                }
            }
        }

        public Deck RetrieveLastDeck()
        {
            Deck lastDeck = null;
            lastDeck = GetDecks().LastOrDefault();

            return lastDeck;
        }

        public async Task CreateDeckFlashCard(DeckFlashCard deckFlashCard)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckFlashCard);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync(_client.BaseAddress + "DeckFlashCard", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckFlashCard = JsonConvert.DeserializeObject<DeckFlashCard>(responseContent);
                }
            }
        }

        public Deck GetDeckByID(int id)
        {
            Deck deck = new Deck();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Deck/{id}?deckid=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deck = JsonConvert.DeserializeObject<Deck>(data);
            }

            return deck;
        }

        public async Task UpdateDeck(Deck deck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PutAsync(_client.BaseAddress + "Deck/{id}?deckid=" + deck.DeckId, content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deck = JsonConvert.DeserializeObject<Deck>(responseContent);
                }
            }
        }

        public async Task DeleteUserDeck(UserDeck userDeck)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(userDeck);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.DeleteAsync(_client.BaseAddress + "UserDeck/" + userDeck.UserId + "/" + userDeck.DeckId))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task DeleteDeckID(int id)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "Deck/{id}?deckid=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
