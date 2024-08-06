using Bot.Api.Services;
using Grpc.Core;
using Shared.GrpcContracts;

namespace Bot.Api.GrpcServices;

public class CreateBotGrpcService : BotCreator.BotCreatorBase
{
    private readonly TelegramBotService _telegramBotService;

    public CreateBotGrpcService(TelegramBotService telegramBotService)
    {
        _telegramBotService = telegramBotService;
    }

    public override async Task<CreateBotResponse> CreateBot(CreateBotRequest request, ServerCallContext context)
    {
        var token = request.Token;
        var result = await _telegramBotService.CreateBotAsync(token);
        return result == 0
            ? new () { Status = 400, Error = "could not create bot" }
            : new () { Status = 200, Error = "" };
    }
}
