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
    }
}