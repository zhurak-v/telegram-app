using Shared;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Api.Factories;

public class TelegramBotFactory
{
    private readonly ILogger<TelegramBotFactory> _logger;

    public TelegramBotFactory(ILogger<TelegramBotFactory> logger)
    {
        _logger = logger;
    }

    public BotInstance Create(string token)
    {
        var cts = new CancellationTokenSource();
        var bot = new TelegramBotClient(token);

        bot.StartReceiving(UpdateHandler, ErrorHandler, cancellationToken: cts.Token);
        
        return new(bot, cts);

        async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is null) return;
            var me = await client.GetMeAsync(cancellationToken);
            Console.WriteLine($"{me.Username} -> ({update.Message.From!.Id}): {update.Message.Text}");
        }

        Task ErrorHandler(ITelegramBotClient telegramBotClient, Exception e, CancellationToken cancellationToken)
        {
            _logger.LogError(e.Message);
            return Task.CompletedTask;
        }
    }
}
