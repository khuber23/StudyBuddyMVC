using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTCStudyBuddy.Data.Models
{
    public class UserDeckGroup
    {
        public int UserDeckGroupId { get; set; }

        public int UserId { get; set; }

        public int DeckGroupId { get; set; }

        public User? User { get; set; }

        public DeckGroup? DeckGroup { get; set; }
    }
}
