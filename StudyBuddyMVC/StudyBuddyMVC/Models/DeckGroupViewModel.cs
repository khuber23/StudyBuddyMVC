﻿using System.ComponentModel.DataAnnotations;

namespace StudyBuddyMVC.Models
{
    public class DeckGroupViewModel
    {
        public int DeckGroupId { get; set; }


        public string? DeckGroupName { get; set; }

        public string? DeckGroupDescription { get; set; }

        public bool IsPublic { get; set; }

        public bool ReadOnly { get; set; }

    }
}
