namespace ApiStudyBuddy.Models
{
	public class Deck
	{
		public int DeckId { get; set; }

		public string? DeckName { get; set; }

		public string? DeckDescription { get; set; }

		public UserDeck? UserDeck { get; set; }

		public List<StudySession>? StudySessions { get; set; }

		public List<DeckFlashCard>? DeckFlashCards { get; set; }

		public List<DeckGroupDeck>? DeckGroupDecks { get; set; }
	}
}
