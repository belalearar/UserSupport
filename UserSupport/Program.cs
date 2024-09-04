using Microsoft.OpenApi.Models;
using UserSupport;
using UserSupport.Common.Configs;
using UserSupport.Common.Services;
using UserSupport.Infrastructure.Services;
using UserSupport.Infrastructure.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddHostedService<SeedDataBackgroundService>();
builder.Services.AddHostedService<UsersMonitorBackgroundService>();
builder.Services.AddHostedService<AgentChatCordinatorService>();
builder.Services.Configure<SystemProfile>(builder.Configuration.GetSection(SystemProfile.Identifier));
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "My API",
//        Version = "v1"
//    });
//});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    // Enable middleware to serve generated Swagger as a JSON endpoint
//    app.UseSwagger();

//    // Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.)
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//    });
//}

app.MapApiEndpoints();
app.UseHttpsRedirection();

app.Run();

public partial class Program { }