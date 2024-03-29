﻿using ApiStudyBuddy.Models;
using Newtonsoft.Json;
using System.Text;

namespace StudyBuddyMVC.Service
{
    public class FlashCardService : IFlashCardService
    {
        Uri baseAddress = new Uri("https://instruct.ntc.edu/studybuddyapi/api/");
        private readonly HttpClient _client = new HttpClient();
        public FlashCardService()
        {
            _client.BaseAddress = baseAddress;
        }

        public List<FlashCard> GetFlashCards()
        {
            List<FlashCard> flashcards = new List<FlashCard>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "FlashCard").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                flashcards = JsonConvert.DeserializeObject<List<FlashCard>>(data);
            }

            return flashcards;
        }

        public async Task CreateFlashCard(FlashCard flashCard)
        {
            var json = JsonConvert.SerializeObject(flashCard);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await _client.PostAsync(_client.BaseAddress + "FlashCard", content))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                flashCard = JsonConvert.DeserializeObject<FlashCard>(responseContent);
            }
        }

        public FlashCard GetFlashCardById(int id)
        {
            FlashCard flashcard = new FlashCard();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "FlashCard/{id}?flashcardid=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                flashcard = JsonConvert.DeserializeObject<FlashCard>(data);
            }

            return flashcard;
        }

        public async Task UpdateFlashCard(FlashCard flashCard)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(flashCard);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await _client.PutAsync(_client.BaseAddress + "FlashCard/{id}?flashcardid=" + flashCard.FlashCardId, content))

                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    flashCard = JsonConvert.DeserializeObject<FlashCard>(responseContent);
                }
            }
        }

        public async Task DeleteFlashCardById(int id)
        {
            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "Flashcard/{id}?flashcardid=" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
