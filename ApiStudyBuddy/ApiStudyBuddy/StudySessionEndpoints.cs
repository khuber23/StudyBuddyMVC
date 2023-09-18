using Microsoft.EntityFrameworkCore;
using ApiStudyBuddy.Data;
using ApiStudyBuddy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace ApiStudyBuddy;

public static class StudySessionEndpoints
{
    public static void MapStudySessionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/StudySession").WithTags(nameof(StudySession));

        group.MapGet("/", async (ApiStudyBuddyContext db) =>
        {
            return await db.StudySessions.ToListAsync();
        })
        .WithName("GetAllStudySessions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<StudySession>, NotFound>> (int studysessionid, ApiStudyBuddyContext db) =>
        {
            return await db.StudySessions.AsNoTracking()
                .FirstOrDefaultAsync(model => model.StudySessionId == studysessionid)
                is StudySession model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetStudySessionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int studysessionid, StudySession studySession, ApiStudyBuddyContext db) =>
        {
            var affected = await db.StudySessions
                .Where(model => model.StudySessionId == studysessionid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.StudySessionId, studySession.StudySessionId)
                    .SetProperty(m => m.StartTime, studySession.StartTime)
                    .SetProperty(m => m.EndTime, studySession.EndTime)
                    .SetProperty(m => m.DeckGroupId, studySession.DeckGroupId)
                    .SetProperty(m => m.DeckId, studySession.DeckId)
                    .SetProperty(m => m.UserId, studySession.UserId)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateStudySession")
        .WithOpenApi();

        group.MapPost("/", async (StudySession studySession, ApiStudyBuddyContext db) =>
        {
            db.StudySessions.Add(studySession);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/StudySession/{studySession.StudySessionId}",studySession);
        })
        .WithName("CreateStudySession")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int studysessionid, ApiStudyBuddyContext db) =>
        {
            var affected = await db.StudySessions
                .Where(model => model.StudySessionId == studysessionid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteStudySession")
        .WithOpenApi();
    }
}
