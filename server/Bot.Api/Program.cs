using System.Net;
using Bot.Api;
using Bot.Api.Factories;
using Bot.Api.GrpcServices;
using Bot.Api.Options;
using Bot.Api.Services;
using Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<BotStarterService>();

builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection("ServiceOptions"));

builder.Services.AddTransient<TelegramBotFactory>();
builder.Services.AddTransient<TelegramBotService>();

builder.Services.AddGrpc();

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.MapGrpcService<CreateBotGrpcService>();

app.Run();