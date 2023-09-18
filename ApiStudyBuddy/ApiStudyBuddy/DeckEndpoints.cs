using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class DeckEndpoints
{
    public static void MapDeckEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Deck").WithTags(nameof(Deck));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.Decks.ToListAsync();
        })
        .WithName("GetAllDecks")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Deck>, NotFound>> (int deckid, ApiStudyBuddyContext db) =>
        {
            return await db.Decks.AsNoTracking()
                .FirstOrDefaultAsync(model => model.DeckId == deckid)
                is Deck model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetDeckById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int deckid, Deck deck, ApiStudyBuddyContext db) =>
        {
            var affected = await db.Decks
                .Where(model => model.DeckId == deckid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeckId, deck.DeckId)
                    .SetProperty(m => m.DeckName, deck.DeckName)
                    .SetProperty(m => m.DeckDescription, deck.DeckDescription)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateDeck")
        .WithOpenApi();

        group.MapPost("/", async (Deck deck, ApiStudyBuddyContext db) =>
        {
            db.Decks.Add(deck);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Deck/{deck.DeckId}",deck);
        })
        .WithName("CreateDeck")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int deckid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.Decks
                .Where(model => model.DeckId == deckid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteDeck")
        .WithOpenApi();
    }
}
