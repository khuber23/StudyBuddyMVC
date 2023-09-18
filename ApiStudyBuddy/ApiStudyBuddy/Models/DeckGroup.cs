namespace ApiStudyBuddy.Models
{
	public class DeckGroup
	{
		public int DeckGroupId { get; set; }

		public string? DeckGroupName { get; set; }

		public string? DeckGroupDescription { get; set; }

		public List<StudySession>? StudySessions { get; set; }

		public UserDeckGroup? UserDeckGroup { get; set; }

		public DeckGroupDeck? DeckGroupDeck { get; set; }
	}
}
