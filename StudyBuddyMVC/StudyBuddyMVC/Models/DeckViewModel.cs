using System.ComponentModel.DataAnnotations;

namespace StudyBuddyMVC.Models
{
    public class DeckViewModel
    {
        public int DeckId { get; set; }

        public string? DeckName { get; set; }

        public string? DeckDescription { get; set; }

        public bool IsPublic { get; set; }

        public bool ReadOnly { get; set; }
    }
}
