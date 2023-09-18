using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class DeckGroupDeckEndpoints
{
    public static void MapDeckGroupDeckEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/DeckGroupDeck").WithTags(nameof(DeckGroupDeck));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.DeckGroupDecks.ToListAsync();
        })
        .WithName("GetAllDeckGroupDecks")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<DeckGroupDeck>, NotFound>> (int deckgroupdeckid, ApiStudyBuddyContext db) =>
        {
            return await db.DeckGroupDecks.AsNoTracking()
                .FirstOrDefaultAsync(model => model.DeckGroupDeckId == deckgroupdeckid)
                is DeckGroupDeck model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetDeckGroupDeckById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int deckgroupdeckid, DeckGroupDeck deckGroupDeck, ApiStudyBuddyContext db) =>
        {
            var affected = await db.DeckGroupDecks
                .Where(model => model.DeckGroupDeckId == deckgroupdeckid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeckGroupDeckId, deckGroupDeck.DeckGroupDeckId)
                    .SetProperty(m => m.DeckGroupId, deckGroupDeck.DeckGroupId)
                    .SetProperty(m => m.DeckId, deckGroupDeck.DeckId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateDeckGroupDeck")
        .WithOpenApi();

        group.MapPost("/", async (DeckGroupDeck deckGroupDeck, ApiStudyBuddyContext db) =>
        {
            db.DeckGroupDecks.Add(deckGroupDeck);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/DeckGroupDeck/{deckGroupDeck.DeckGroupDeckId}",deckGroupDeck);
        })
        .WithName("CreateDeckGroupDeck")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int deckgroupdeckid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.DeckGroupDecks
                .Where(model => model.DeckGroupDeckId == deckgroupdeckid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteDeckGroupDeck")
        .WithOpenApi();
    }
}
