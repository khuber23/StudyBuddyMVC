﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtcStudyBuddy.DataAccess.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        //public void ProfilePicture { get; set; }

        public List<UserDeck>? UserDecks { get; set; }

        public List<UserDeckGroup>? UserDeckGroups { get; set; }

        public List<StudySession>? StudySessions { get; set;}
    }
}
