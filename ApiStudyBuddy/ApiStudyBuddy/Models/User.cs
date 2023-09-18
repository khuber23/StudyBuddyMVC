using System.ComponentModel.DataAnnotations;

namespace ApiStudyBuddy.Models
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

		public List<StudySession>? StudySessions { get; set; }
	}
}
