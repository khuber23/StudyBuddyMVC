using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTCStudyBuddy.DataAccess.Models
{
    public class UserDeck
    {
        public int UserDeckId { get; set; }

        public int UserId { get; set; }

        public int DeckId { get; set; }

        public User? User { get; set; }

        public Deck? Deck { get; set; }

        public StudySession? StudySession { get; set; }
    }
}
