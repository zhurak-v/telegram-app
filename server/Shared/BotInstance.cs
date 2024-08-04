using Telegram.Bot;

namespace Shared;

public record BotInstance(TelegramBotClient Bot, CancellationTokenSource CancellationTokenSource);