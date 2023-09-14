using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTCStudyBuddy.Data.Models
{
    public class DeckGroupDeck
    {
        public int DeckGroupDeckId { get; set; }

        public int DeckGroupId { get; set; }

        public int DeckId { get; set; }

        public DeckGroup? DeckGroup { get; set; }

        public Deck? Deck { get; set; }
    }
}
