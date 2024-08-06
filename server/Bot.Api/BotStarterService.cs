using Bot.Api.Options;
using Bot.Api.Services;
using Microsoft.Extensions.Options;
using Shared;

namespace Bot.Api;

public class BotStarterService : BackgroundService
{
    private readonly ILogger<BotStarterService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IOptions<ServiceOptions> _options;

    public BotStarterService(
        ILogger<BotStarterService> logger, 
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
        var botsData = await telegramBotService.GetBotsAsync();
        
        foreach (var botData in botsData)
        {
            var token = Encryptor.Decrypt(botData.EncryptedToken, _options.Value.Key);
            var id = await telegramBotService.CreateBotAsync(token);
            _logger.LogInformation($"{botData.Id} ({id} STARTED)");
        }

        _logger.LogInformation("ALL BOTS STARTED");
    }
}
