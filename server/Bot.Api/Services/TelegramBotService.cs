using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Api.Factories;
using Bot.Api.Options;
using Domain.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace Bot.Api.Services;

public class TelegramBotService(
    TelegramBotFactory telegramBotFactory,
    IOptions<ServiceOptions> _options,
    AppDbContext appDbContext)
{
    public async Task<long> CreateBotAsync(string token)
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

    public async Task<List<TelegramBot>> GetBotsAsync() => 
        await appDbContext.TelegramBots.AsNoTracking().ToListAsync();
}
