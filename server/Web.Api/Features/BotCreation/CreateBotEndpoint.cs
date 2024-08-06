using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Shared.GrpcContracts;
using Web.Api.Extensions;

namespace Web.Api.Features.BotCreation;

public class CreateBotEndpoint : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("bot", async ([FromBody] CreateBotDto dto) =>
        {
            using var channel = GrpcChannel.ForAddress("http://botservice:8081");
            var client = new BotCreator.BotCreatorClient(channel);
            return await client.CreateBotAsync(new () { Token = dto.Token });
        });
        
        return endpoints;
    }
}
