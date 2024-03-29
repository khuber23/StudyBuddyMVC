﻿using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudyBuddyMVC.Models
{
    public class DecksViewModel
    {
        public int DeckId { get; set; }
        public List<SelectListItem>? Decks { get; set; }

        public List<SelectListItem>? Users { get; set; }

        public int UserId { get; set; }

        public FlashCard? FlashCard { get; set; }
    }
}
