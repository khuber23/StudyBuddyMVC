using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtcStudyBuddy.DataAccess.Models
{
    public class StudySession
    {
        public int StudySessionId { get; set; } 

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int DeckGroupId { get; set; }

        public int DeckId { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public Deck? Deck { get; set; }

        public DeckGroup? DeckGroup { get; set; }

        public StudySessionFlashCard? StudySessionFlashCard { get; set; }
    }
}
