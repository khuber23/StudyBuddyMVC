using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NtcStudyBuddy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeckGroups",
                columns: table => new
                {
                    DeckGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckGroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeckGroupDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckGroups", x => x.DeckGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    DeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeckDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.DeckId);
                });

            migrationBuilder.CreateTable(
                name: "FlashCards",
                columns: table => new
                {
                    FlashCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlashCardQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlashCardAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCards", x => x.FlashCardId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "DeckGroupDecks",
                columns: table => new
                {
                    DeckGroupDeckId = table.Column<int>(type: "int", nullable: false),
                    DeckGroupId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckGroupDecks", x => x.DeckGroupDeckId);
                    table.ForeignKey(
                        name: "FK_DeckGroupDecks_DeckGroups_DeckGroupDeckId",
                        column: x => x.DeckGroupDeckId,
                        principalTable: "DeckGroups",
                        principalColumn: "DeckGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckGroupDecks_Decks_DeckGroupDeckId",
                        column: x => x.DeckGroupDeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckFlashCards",
                columns: table => new
                {
                    DeckFlashCardId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    FlashCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckFlashCards", x => x.DeckFlashCardId);
                    table.ForeignKey(
                        name: "FK_DeckFlashCards_Decks_DeckFlashCardId",
                        column: x => x.DeckFlashCardId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckFlashCards_FlashCards_DeckFlashCardId",
                        column: x => x.DeckFlashCardId,
                        principalTable: "FlashCards",
                        principalColumn: "FlashCardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudySessions",
                columns: table => new
                {
                    StudySessionId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeckGroupId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySessions", x => x.StudySessionId);
                    table.ForeignKey(
                        name: "FK_StudySessions_DeckGroups_StudySessionId",
                        column: x => x.StudySessionId,
                        principalTable: "DeckGroups",
                        principalColumn: "DeckGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudySessions_Decks_StudySessionId",
                        column: x => x.StudySessionId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudySessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDeckGroups",
                columns: table => new
                {
                    UserDeckGroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeckGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDeckGroups", x => x.UserDeckGroupId);
                    table.ForeignKey(
                        name: "FK_UserDeckGroups_DeckGroups_UserDeckGroupId",
                        column: x => x.UserDeckGroupId,
                        principalTable: "DeckGroups",
                        principalColumn: "DeckGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDeckGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDecks",
                columns: table => new
                {
                    UserDeckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDecks", x => x.UserDeckId);
                    table.ForeignKey(
                        name: "FK_UserDecks_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "DeckId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDecks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudySessionsFlashCards",
                columns: table => new
                {
                    StudySessionFlashCardId = table.Column<int>(type: "int", nullable: false),
                    StudySessionId = table.Column<int>(type: "int", nullable: false),
                    FlashCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySessionsFlashCards", x => x.StudySessionFlashCardId);
                    table.ForeignKey(
                        name: "FK_StudySessionsFlashCards_FlashCards_StudySessionFlashCardId",
                        column: x => x.StudySessionFlashCardId,
                        principalTable: "FlashCards",
                        principalColumn: "FlashCardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudySessionsFlashCards_StudySessions_StudySessionFlashCardId",
                        column: x => x.StudySessionFlashCardId,
                        principalTable: "StudySessions",
                        principalColumn: "StudySessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DeckGroups",
                columns: new[] { "DeckGroupId", "DeckGroupDescription", "DeckGroupName" },
                values: new object[] { 1, "", "Design Patterns" });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "DeckId", "DeckDescription", "DeckName" },
                values: new object[,]
                {
                    { 1, "Design patterns all about class instantiation", "Creational Design Patterns" },
                    { 2, "Design patterns all about class and Object composition", "Structural Design Patterns" },
                    { 3, "Design patterns all about Class's objects communication", "Behavorial Design Patterns" }
                });

            migrationBuilder.InsertData(
                table: "FlashCards",
                columns: new[] { "FlashCardId", "FlashCardAnswer", "FlashCardQuestion" },
                values: new object[,]
                {
                    { 1, "Creates an instance of several families of classes", "What is abstract factory" },
                    { 2, "A class of which only a single instance can exist", "What is Singleton?" },
                    { 3, "Add responsibilites to objects dynamically", "What is decorator?" },
                    { 4, "A single class that represents an entire subsystem", "What is facade?" },
                    { 5, "Sequentially access the elements of a collection", "What is iterator?" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "Password", "Username" },
                values: new object[,]
                {
                    { 1, "JohnDoe@gmail.com", "John", "Doe", "1234", "JDoe1" },
                    { 2, "MaryJane@gmail.com", "Mary", "Jane", "4321", "MJane1" }
                });

            migrationBuilder.InsertData(
                table: "DeckFlashCards",
                columns: new[] { "DeckFlashCardId", "DeckId", "FlashCardId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "DeckGroupDecks",
                columns: new[] { "DeckGroupDeckId", "DeckGroupId", "DeckId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "StudySessions",
                columns: new[] { "StudySessionId", "DeckGroupId", "DeckId", "EndTime", "StartTime", "UserId" },
                values: new object[] { 1, 0, 1, new DateTime(2023, 9, 11, 15, 35, 15, 0, DateTimeKind.Unspecified), new DateTime(2023, 9, 11, 15, 5, 15, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "UserDeckGroups",
                columns: new[] { "UserDeckGroupId", "DeckGroupId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "UserDecks",
                columns: new[] { "UserDeckId", "DeckId", "UserId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "StudySessionsFlashCards",
                columns: new[] { "StudySessionFlashCardId", "FlashCardId", "StudySessionId" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_UserId",
                table: "StudySessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeckGroups_UserId",
                table: "UserDeckGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDecks_DeckId",
                table: "UserDecks",
                column: "DeckId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDecks_UserId",
                table: "UserDecks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckFlashCards");

            migrationBuilder.DropTable(
                name: "DeckGroupDecks");

            migrationBuilder.DropTable(
                name: "StudySessionsFlashCards");

            migrationBuilder.DropTable(
                name: "UserDeckGroups");

            migrationBuilder.DropTable(
                name: "UserDecks");

            migrationBuilder.DropTable(
                name: "FlashCards");

            migrationBuilder.DropTable(
                name: "StudySessions");

            migrationBuilder.DropTable(
                name: "DeckGroups");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
