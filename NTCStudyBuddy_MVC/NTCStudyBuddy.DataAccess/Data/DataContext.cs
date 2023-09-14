using Microsoft.EntityFrameworkCore;
using NTCStudyBuddy.DataAccess.Models;

namespace NTCStudyBuddy.DataAccess.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationship between User(1) and UserDeck (many)
            modelBuilder.Entity<UserDeck>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserDecks)
                .HasForeignKey(x => x.UserId);

            // Define relationship between User(1) and UserDeckGroup (many)
            modelBuilder.Entity<UserDeckGroup>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserDeckGroups)
                .HasForeignKey(x => x.UserId);

            // Define relationship between User(1) and StudySession (many)
            modelBuilder.Entity<StudySession>()
                .HasOne(x => x.User)
                .WithMany(x => x.StudySessions)
                .HasForeignKey(x => x.UserId);

            // =================================================================

            // Define relationship between Deck(1) and UserDeck (1)
            modelBuilder.Entity<UserDeck>()
                .HasOne(x => x.Deck)
                .WithOne(x => x.UserDeck)
                .HasForeignKey<UserDeck>(x => x.DeckId)
                .IsRequired();

            // Define relationship between Deck(1) and StudySession (many)
            modelBuilder.Entity<StudySession>()
                .HasOne(x => x.Deck)
                .WithMany(x => x.StudySessions)
                .HasForeignKey(x => x.StudySessionId);

            // Define relationship between Deck(1) and DeckGroupDeck (many)
            modelBuilder.Entity<DeckGroupDeck>()
                .HasOne(x => x.Deck)
                .WithMany(x => x.DeckGroupDecks)
                .HasForeignKey(x => x.DeckGroupDeckId);

            // Define relationship between Deck(1) and DeckFlashCard (many)
            modelBuilder.Entity<DeckFlashCard>()
                .HasOne(x => x.Deck)
                .WithMany(x => x.DeckFlashCards)
                .HasForeignKey(x => x.DeckFlashCardId);

            // =================================================================

            // Define relationship between DeckGroup(1) and StudySession (many)
            modelBuilder.Entity<StudySession>()
                .HasOne(x => x.DeckGroup)
                .WithMany(x => x.StudySessions)
                .HasForeignKey(x => x.StudySessionId);

            // Define relationship between DeckGroup(1) and UserDeckGroup(1)
            modelBuilder.Entity<UserDeckGroup>()
                .HasOne(x => x.DeckGroup)
                .WithOne(x => x.UserDeckGroup)
                .HasForeignKey<UserDeckGroup>(x => x.UserDeckGroupId)
                .IsRequired();

            // Define relationship between DeckGroup(1) and DeckGroupDeck(1)
            modelBuilder.Entity<DeckGroupDeck>()
                .HasOne(x => x.DeckGroup)
                .WithOne(x => x.DeckGroupDeck)
                .HasForeignKey<DeckGroupDeck>(x => x.DeckGroupDeckId)
                .IsRequired();

            // =================================================================

            // Define relationship between StudySession(1) and StudySessionFlashCard (1)
            modelBuilder.Entity<StudySessionFlashCard>()
                .HasOne(x => x.StudySession)
                .WithOne(x => x.StudySessionFlashCard)
                .HasForeignKey<StudySessionFlashCard>(x => x.StudySessionFlashCardId)
                .IsRequired();

            // =================================================================

            // Define relationship between FlashCard(1) and DeckFlashCard(1)
            modelBuilder.Entity<DeckFlashCard>()
                .HasOne(x => x.FlashCard)
                .WithOne(x => x.DeckFlashCard)
                .HasForeignKey<DeckFlashCard>(x => x.DeckFlashCardId)
                .IsRequired();

            // Define relationship between FlashCard(1) and StudySessionFlashCard(1)
            modelBuilder.Entity<StudySessionFlashCard>()
                .HasOne(x => x.FlashCard)
                .WithOne(x => x.StudySessionFlashCard)
                .HasForeignKey<StudySessionFlashCard>(x => x.StudySessionFlashCardId)
                .IsRequired();


            // Create data for database
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "John.Doe@gmail.com", Username = "JDoe1", Password = "1234" },
                new User { UserId = 1, FirstName = "Mary", LastName = "Jane", Email = "Mary.Jane@gmail.com", Username = "MJane1", Password = "4321" }
                );

            modelBuilder.Entity<StudySession>().HasData(
                new StudySession { StudySessionId = 1, 
                                    StartTime = DateTime.Parse("09/11/2023 03:05:15 PM"),
                                    EndTime = DateTime.Parse("09/11/2023 03:35:15 PM"),  
                                    DeckId = 1, 
                                    UserId = 1  }
                );

            modelBuilder.Entity<Deck>().HasData(
                new Deck { DeckId= 1, DeckName = "Creational Design Patterns", DeckDescription = "Design patterns all about class instantiation" },
                new Deck { DeckId = 2, DeckName = "Structural Design Patterns", DeckDescription = "Design patterns all about class and Object composition" },
                new Deck { DeckId = 3, DeckName = "Behavorial Design Patterns", DeckDescription = "Design patterns all about Class's objects communication" }
                );

            modelBuilder.Entity<DeckGroup>().HasData(
                new DeckGroup { DeckGroupId = 1, DeckGroupName = "Design Patterns", DeckGroupDescription = ""}
                );

            modelBuilder.Entity<FlashCard>().HasData(
                new FlashCard { FlashCardId = 1, FlashCardQuestion = "What is abstract factory", FlashCardAnswer = "Creates an instance of several families of classes" },
                new FlashCard { FlashCardId = 2, FlashCardQuestion = "What is Singleton?", FlashCardAnswer = "A class of which only a single instance can exist" },
                new FlashCard { FlashCardId = 3, FlashCardQuestion = "What is decorator?", FlashCardAnswer = "Add responsibilites to objects dynamically" },
                new FlashCard { FlashCardId = 4, FlashCardQuestion = "What is facade?", FlashCardAnswer = "A single class that represents an entire subsystem" },
                new FlashCard { FlashCardId = 5, FlashCardQuestion = "What is iterator?", FlashCardAnswer = "Sequentially access the elements of a collection" }
                );

            modelBuilder.Entity<UserDeck>().HasData(
                new UserDeck { UserDeckId = 1, UserId = 1, DeckId = 1}
                );

            modelBuilder.Entity<UserDeckGroup>().HasData(
                new UserDeckGroup { UserDeckGroupId = 1, UserId = 1, DeckGroupId = 1 }
                );

            modelBuilder.Entity<DeckGroupDeck>().HasData(
                new DeckGroupDeck { DeckGroupDeckId = 1, DeckGroupId = 1, DeckId = 1 }
                );

            modelBuilder.Entity<DeckFlashCard>().HasData(
                new DeckFlashCard { DeckFlashCardId = 1, DeckId = 1, FlashCardId = 1, }
                );

            modelBuilder.Entity<StudySessionFlashCard>().HasData(
                new StudySessionFlashCard { StudySessionFlashCardId = 1, StudySessionId = 1, FlashCardId = 1 }
                );

        }


        public DbSet<User> Users { get; set; }
        public DbSet<StudySession> StudySessions { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<DeckGroup> DeckGroups { get; set; }
        public DbSet<FlashCard> FlashCards { get; set; }

        // Transitional Tables 
        public DbSet<UserDeck> UserDecks { get; set; }
        public DbSet<UserDeckGroup> UserDeckGroups { get; set; }
        public DbSet<DeckGroupDeck> DeckGroupDecks { get; set; }
        public DbSet<DeckFlashCard> DeckFlashCards { get; set; }
        public DbSet<StudySessionFlashCard> StudySessionsFlashCards { get; set; }
        
    }
}
