using Bot.AppHost.Factories;
using Bot.AppHost.Options;
using Domain.Entities;
using Infrastructure;
using Microsoft.Extensions.Options;
using Shared;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Bot.AppHost.Services;

public class TelegramBotService(
    TelegramBotFactory telegramBotFactory,
    IOptions<ServiceOptions> _options,
    AppDbContext appDbContext)
{
    public async Task<long> CreateBot(string token)
    {
        try
        {
            var instance = telegramBotFactory.Create(token);
            var id = (await instance.Bot.GetMeAsync()).Id;

            if (appDbContext.TelegramBots.Any(t => t.BotId == id))
                return id;
            
            var entity = new TelegramBot
            {
                BotId = id,
                CreatedAt = DateTime.Now,
                EncryptedToken = Encryptor.Encrypt(token, _options.Value.Key)
            };

            await appDbContext.TelegramBots.AddAsync(entity);
            await appDbContext.SaveChangesAsync();
            
            return id;
        }
        catch (ApiRequestException)
        {
            return 0;
        }
    }
}
