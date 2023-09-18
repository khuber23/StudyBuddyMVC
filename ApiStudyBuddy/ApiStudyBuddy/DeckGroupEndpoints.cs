using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class DeckGroupEndpoints
{
    public static void MapDeckGroupEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/DeckGroup").WithTags(nameof(DeckGroup));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.DeckGroups.ToListAsync();
        })
        .WithName("GetAllDeckGroups")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<DeckGroup>, NotFound>> (int deckgroupid, ApiStudyBuddyContext db) =>
        {
            return await db.DeckGroups.AsNoTracking()
                .FirstOrDefaultAsync(model => model.DeckGroupId == deckgroupid)
                is DeckGroup model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetDeckGroupById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int deckgroupid, DeckGroup deckGroup, ApiStudyBuddyContext db) =>
        {
            var affected = await db.DeckGroups
                .Where(model => model.DeckGroupId == deckgroupid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeckGroupId, deckGroup.DeckGroupId)
                    .SetProperty(m => m.DeckGroupName, deckGroup.DeckGroupName)
                    .SetProperty(m => m.DeckGroupDescription, deckGroup.DeckGroupDescription)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateDeckGroup")
        .WithOpenApi();

        group.MapPost("/", async (DeckGroup deckGroup, ApiStudyBuddyContext db) =>
        {
            db.DeckGroups.Add(deckGroup);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/DeckGroup/{deckGroup.DeckGroupId}",deckGroup);
        })
        .WithName("CreateDeckGroup")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int deckgroupid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.DeckGroups
                .Where(model => model.DeckGroupId == deckgroupid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteDeckGroup")
        .WithOpenApi();
    }
}
