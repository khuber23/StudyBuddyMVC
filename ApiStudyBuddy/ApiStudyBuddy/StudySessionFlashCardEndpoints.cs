using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class StudySessionFlashCardEndpoints
{
    public static void MapStudySessionFlashCardEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/StudySessionFlashCard").WithTags(nameof(StudySessionFlashCard));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.StudySessionsFlashCards.ToListAsync();
        })
        .WithName("GetAllStudySessionFlashCards")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<StudySessionFlashCard>, NotFound>> (int studysessionflashcardid, ApiStudyBuddyContext db) =>
        {
            return await db.StudySessionsFlashCards.AsNoTracking()
                .FirstOrDefaultAsync(model => model.StudySessionFlashCardId == studysessionflashcardid)
                is StudySessionFlashCard model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetStudySessionFlashCardById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int studysessionflashcardid, StudySessionFlashCard studySessionFlashCard, ApiStudyBuddyContext db) =>
        {
            var affected = await db.StudySessionsFlashCards
                .Where(model => model.StudySessionFlashCardId == studysessionflashcardid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.StudySessionFlashCardId, studySessionFlashCard.StudySessionFlashCardId)
                    .SetProperty(m => m.StudySessionId, studySessionFlashCard.StudySessionId)
                    .SetProperty(m => m.FlashCardId, studySessionFlashCard.FlashCardId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateStudySessionFlashCard")
        .WithOpenApi();

        group.MapPost("/", async (StudySessionFlashCard studySessionFlashCard, ApiStudyBuddyContext db) =>
        {
            db.StudySessionsFlashCards.Add(studySessionFlashCard);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/StudySessionFlashCard/{studySessionFlashCard.StudySessionFlashCardId}",studySessionFlashCard);
        })
        .WithName("CreateStudySessionFlashCard")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int studysessionflashcardid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.StudySessionsFlashCards
                .Where(model => model.StudySessionFlashCardId == studysessionflashcardid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteStudySessionFlashCard")
        .WithOpenApi();
    }
}
