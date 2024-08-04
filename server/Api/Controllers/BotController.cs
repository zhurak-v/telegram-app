using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.MassTransitContracts;

namespace Api.Controllers;

public record CreateBotDto
{
    public string? Token { get; set; }
}

[ApiController]
[Route("api/bot")]
public class BotController(IBus bus) : ControllerBase
{
    [HttpPost]
    public async Task CreateBot(CreateBotDto dto)
    {
        var message = new CreateBotMessage(dto.Token!);
        await bus.Publish(message);
    }
}
