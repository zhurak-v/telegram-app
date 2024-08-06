using Bot.AppHost.Options;
using Bot.AppHost.Services;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared;

namespace Bot.AppHost;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IOptions<ServiceOptions> _options;

    public Worker(
        ILogger<Worker> logger, 
        IOptions<ServiceOptions> options,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _options = options;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var telegramBotService = scope.ServiceProvider.GetRequiredService<TelegramBotService>();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        var data = appDbContext.TelegramBots.AsNoTracking().ToList();
        foreach (var botData in data)
        {
            var token = Encryptor.Decrypt(botData.EncryptedToken, _options.Value.Key);
            await telegramBotService.CreateBot(token);
            _logger.LogInformation($"{botData.Id} ({botData.BotId} STARTED)");
        }

        _logger.LogInformation("ALL BOTS STARTED");
    }
}
