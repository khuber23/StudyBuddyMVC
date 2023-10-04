using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTCStudyBuddy.DataAccess.Models
{
    public class DeckFlashCard
    {
        public int DeckFlashCardId { get; set; }

        public int DeckId { get; set; }

        public int FlashCardId { get; set; }

        public Deck? Deck { get; set; }

        public FlashCard? FlashCard { get; set; }
    }
}
