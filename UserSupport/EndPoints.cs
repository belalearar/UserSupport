using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using UserSupport.Common.Services;

namespace UserSupport
{
    [ExcludeFromCodeCoverage]
    public static class EndPoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            app.MapPost("create-session", CreateSession);
            app.MapPost("poll", Poll);

            app.MapGet("queue", GetQueue);
            app.MapGet("team-on-shift", GetCurrentTeam);
            app.MapGet("team-overflow", GetOverflowTeam);
        }

        static IResult CreateSession([FromBody] string userName, ISessionService sessionServeic, CancellationToken cancellationToken)
        {
            var result = sessionServeic.CreateSession(userName);
            return result ? Results.Created() : Results.BadRequest("Sorry Chat Is Full");
        }

        static IResult Poll([FromBody] string userName, ISessionService sessionServeic, CancellationToken cancellationToken)
        {
            sessionServeic.Poll(userName);
            return Results.NoContent();
        }

        static IResult GetQueue(IChatService chatService, CancellationToken cancellationToken)
        {
            var queue = chatService.GetQueue();
            return Results.Ok(queue);
        }

        static IResult GetCurrentTeam(IChatService chatService, CancellationToken cancellationToken)
        {
            var queue = chatService.GetOnShiftTeam();
            return Results.Ok(queue);
        }

        static IResult GetOverflowTeam(IChatService chatService, CancellationToken cancellationToken)
        {
            var queue = chatService.GetOverflowTeam();
            return Results.Ok(queue);
        }
    }
}