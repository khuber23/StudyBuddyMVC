namespace ApiStudyBuddy.Models
{
	public class UserDeck
	{
		public int UserDeckId { get; set; }

		public int UserId { get; set; }

		public int DeckId { get; set; }

		public User? User { get; set; }

		public Deck? Deck { get; set; }
	}
}
