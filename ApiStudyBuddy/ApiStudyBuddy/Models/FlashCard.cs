namespace ApiStudyBuddy.Models
{
	public class FlashCard
	{
		public int FlashCardId { get; set; }

		public string? FlashCardQuestion { get; set; }

		public string? FlashCardAnswer { get; set; }

		//public int FlashCardQuestionImage { get; set; }

		//public string FlashCardAnswerImage { get; set; }

		public DeckFlashCard? DeckFlashCard { get; set; }

		public StudySessionFlashCard? StudySessionFlashCard { get; set; }
	}
}
