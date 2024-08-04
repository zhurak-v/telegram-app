using Shared;
using Telegram.Bot;

namespace Bot.AppHost.Factories;

public class TelegramBotFactory
{
    public BotInstance Create(string token)
    {
        var cts = new CancellationTokenSource();
        var bot = new TelegramBotClient(token);
        
        bot.StartReceiving(async (client, update, cancellationToken) =>
        {
            if(update.Message is null) return;
            var me = await client.GetMeAsync(cancellationToken: cancellationToken);
            Console.WriteLine($"{me.Username} -> ({update.Message.From!.Id}): {update.Message.Text}");    
            
        }, (_, _, _) => { }, cancellationToken: cts.Token);
        
        return new(bot, cts);
    }
}
