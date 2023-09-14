using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTCStudyBuddy.Data.Models
{
    public class StudySessionFlashCard
    {
        public int StudySessionFlashCardId { get; set; }

        public int StudySessionId { get; set; }

        public int FlashCardId { get; set; }

        public StudySession? StudySession { get; set; }

        public FlashCard? FlashCard { get; set; }
    }
}
