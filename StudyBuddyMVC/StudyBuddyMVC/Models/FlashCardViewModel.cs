using System.ComponentModel.DataAnnotations;

namespace StudyBuddyMVC.Models
{
    public class FlashCardViewModel
    {
        public int FlashCardID {  get; set; }

        [Required]
        public string FlashCardQuestion {  get; set; }

        [Required]
        public string FlashCardAnswer { get; set; }

        public string? FlashCardAnswerImage { get; set; }

        public bool IsPublic { get; set; }

        public bool ReadOnly { get; set; }

        public int DeckID {  get; set; }
    }
}
