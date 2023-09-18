using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class UserDeckEndpoints
{
    public static void MapUserDeckEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/UserDeck").WithTags(nameof(UserDeck));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.UserDecks.ToListAsync();
        })
        .WithName("GetAllUserDecks")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<UserDeck>, NotFound>> (int userdeckid, ApiStudyBuddyContext db) =>
        {
            return await db.UserDecks.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserDeckId == userdeckid)
                is UserDeck model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserDeckById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int userdeckid, UserDeck userDeck, ApiStudyBuddyContext db) =>
        {
            var affected = await db.UserDecks
                .Where(model => model.UserDeckId == userdeckid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.UserDeckId, userDeck.UserDeckId)
                    .SetProperty(m => m.UserId, userDeck.UserId)
                    .SetProperty(m => m.DeckId, userDeck.DeckId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUserDeck")
        .WithOpenApi();

        group.MapPost("/", async (UserDeck userDeck, ApiStudyBuddyContext db) =>
        {
            db.UserDecks.Add(userDeck);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/UserDeck/{userDeck.UserDeckId}",userDeck);
        })
        .WithName("CreateUserDeck")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int userdeckid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.UserDecks
                .Where(model => model.UserDeckId == userdeckid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUserDeck")
        .WithOpenApi();
    }
}
