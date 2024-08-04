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
    private readonly AppDbContext _appDbContext;
    private readonly IOptions<ServiceOptions> _options;
    private readonly TelegramBotService _telegramBotService;

    public Worker(
        ILogger<Worker> logger, 
        AppDbContext appDbContext,
        IOptions<ServiceOptions> options,
        TelegramBotService telegramBotService)
    {
        _logger = logger;
        _appDbContext = appDbContext;
        _options = options;
        _telegramBotService = telegramBotService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var data = _appDbContext.TelegramBots.AsNoTracking();
        foreach (var botData in data)
        {
            var token = Encryptor.Decrypt(botData.EncryptedToken, _options.Value.Key);
            await _telegramBotService.CreateBot(token);
            _logger.LogInformation($"{botData.Id} ({botData.BotId} STARTED)");
        }
        
        _logger.LogInformation("ALL BOTS STARTED");
    }
}
