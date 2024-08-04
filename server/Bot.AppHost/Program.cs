using Bot.AppHost.Factories;
using Bot.AppHost.Options;
using Bot.AppHost.RabbitMQ;
using Bot.AppHost.Services;
using Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();

builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection("ServiceOptions"));

builder.Services.AddTransient<TelegramBotFactory>();
builder.Services.AddTransient<TelegramBotService>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateBotConsumer>();
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h => 
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(ctx);    
    });
});

var host = builder.Build();

{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

host.Run();
