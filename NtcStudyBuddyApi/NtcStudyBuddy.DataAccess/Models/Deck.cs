using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtcStudyBuddy.DataAccess.Models
{
    public class Deck
    {
        public int DeckId { get; set; }

        public string? DeckName { get; set; }

        public string? DeckDescription { get; set; }

        public UserDeck? UserDeck { get; set; }

        public List<StudySession>? StudySessions { get; set; }

        public List<DeckFlashCard>? DeckFlashCards { get; set; }

        public List<DeckGroupDeck>? DeckGroupDecks { get; set;}
    }
}
