using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class DeckFlashCardEndpoints
{
    public static void MapDeckFlashCardEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/DeckFlashCard").WithTags(nameof(DeckFlashCard));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.DeckFlashCards.ToListAsync();
        })
        .WithName("GetAllDeckFlashCards")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<DeckFlashCard>, NotFound>> (int deckflashcardid, ApiStudyBuddyContext db) =>
        {
            return await db.DeckFlashCards.AsNoTracking()
                .FirstOrDefaultAsync(model => model.DeckFlashCardId == deckflashcardid)
                is DeckFlashCard model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetDeckFlashCardById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int deckflashcardid, DeckFlashCard deckFlashCard, ApiStudyBuddyContext db) =>
        {
            var affected = await db.DeckFlashCards
                .Where(model => model.DeckFlashCardId == deckflashcardid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeckFlashCardId, deckFlashCard.DeckFlashCardId)
                    .SetProperty(m => m.DeckId, deckFlashCard.DeckId)
                    .SetProperty(m => m.FlashCardId, deckFlashCard.FlashCardId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateDeckFlashCard")
        .WithOpenApi();

        group.MapPost("/", async (DeckFlashCard deckFlashCard, ApiStudyBuddyContext db) =>
        {
            db.DeckFlashCards.Add(deckFlashCard);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/DeckFlashCard/{deckFlashCard.DeckFlashCardId}",deckFlashCard);
        })
        .WithName("CreateDeckFlashCard")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int deckflashcardid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.DeckFlashCards
                .Where(model => model.DeckFlashCardId == deckflashcardid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteDeckFlashCard")
        .WithOpenApi();
    }
}
