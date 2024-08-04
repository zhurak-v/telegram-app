using Bot.AppHost.Services;
using MassTransit;
using Shared.MassTransitContracts;

namespace Bot.AppHost.RabbitMQ;

public class CreateBotConsumer : IConsumer<CreateBotMessage>
{
    private readonly TelegramBotService _telegramBotService;

    public CreateBotConsumer(TelegramBotService telegramBotService)
    {
        _telegramBotService = telegramBotService;
    }

    public async Task Consume(ConsumeContext<CreateBotMessage> context)
    {
        var token = context.Message.Token;
        await _telegramBotService.CreateBot(token);
    }
}