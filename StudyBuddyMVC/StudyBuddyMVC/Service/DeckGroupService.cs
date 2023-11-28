using ApiStudyBuddy.Models;
using Newtonsoft.Json;
using System.Text;

namespace StudyBuddyMVC.Service
{
    public class DeckGroupService : IDeckGroupService
    {
        Uri baseAddress = new Uri("https://localhost:7025/api/");
        private readonly HttpClient _client = new HttpClient();

        public DeckGroupService()
        {
            _client.BaseAddress = baseAddress;
        }

        public List<DeckGroup> GetDeckGroups()
        {
            List<DeckGroup> deckgroups = new List<DeckGroup>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "DeckGroup").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroups = JsonConvert.DeserializeObject<List<DeckGroup>>(data);
            }

            return deckgroups;
        }

        public async Task CreateDeckGroup(DeckGroup deckGroup)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckGroup);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PostAsync(_client.BaseAddress + "DeckGroup", content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckGroup = JsonConvert.DeserializeObject<DeckGroup>(responseContent);
                }
            }
        }

        public DeckGroup RetrieveLastDeckGroup()
        {
            DeckGroup deckGroup = null;

            deckGroup = GetDeckGroups().LastOrDefault();
            return deckGroup;
        }

        public DeckGroup GetDeckGroupByID(int id)
        {
            DeckGroup deckgroup = new DeckGroup();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "DeckGroup/{id}?deckgroupid=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                deckgroup = JsonConvert.DeserializeObject<DeckGroup>(data);
            }

            return deckgroup;
        }

        public async Task UpdateDeckGroup(DeckGroup deckGroup)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(deckGroup);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PutAsync(_client.BaseAddress + "DeckGroup/{id}?deckgroupid=" + deckGroup.DeckGroupId, content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    deckGroup = JsonConvert.DeserializeObject<DeckGroup>(responseContent);
                }
            }
        }
    }
}
